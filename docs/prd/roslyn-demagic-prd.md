# PRD: Roslyn.DeMagic

**Product:** `Roslyn.DeMagic`  
**Suite:** `roslyn-lint`  
**Type:** Roslyn Diagnostic Analyzer (NuGet)  
**Status:** Draft v0.1  
**Author:** Rand Lee

---

## Overview

`Roslyn.DeMagic` is the first analyzer in the `roslyn-lint` suite. It enforces two classes of code hygiene rules that prevent domain coupling and structural decay in C# codebases: constant consolidation and forbidden string literal values.

The name reflects the core mission — eliminating "magic" values and implicit domain knowledge from code that should be general-purpose.

---

## Problem Statement

As general-purpose tools and libraries grow, two failure modes emerge:

1. **Constant sprawl** — public and internal constants declared inline across many files, making them difficult to audit, refactor, or govern.
2. **Domain bleed** — domain-specific string values (product names, internal identifiers, environment-specific strings) embedded in general tooling, creating invisible coupling that is hard to detect via code review.

Both failures are invisible to standard Roslyn analyzers. `Roslyn.DeMagic` makes them visible and enforceable.

---

## Goals

- Provide two configurable diagnostic rules surfaced as standard Roslyn diagnostics
- Zero-friction integration: works with any .NET project that includes the NuGet package
- Config-driven: all rule behavior defined in `.roslyn-lint/demagic.toml`
- Severity is configurable per rule
- No runtime dependency — analyzer only

---

## Non-Goals

- Does not analyze non-const string literals for consolidation (Rule DM001 is const-scoped)
- Does not enforce naming conventions for constants
- Does not auto-fix violations (code fix providers are out of scope for v1)
- Does not analyze values inside comments, XML doc, or string interpolation holes (v1)

---

## Configuration

All rule configuration lives in `.roslyn-lint/` at the solution root. There are two independent config files — one for src projects, one for test projects. Each is fully self-contained; there is no inheritance or merging between them.

### Files

```
.roslyn-lint/
  config-src.toml    ← applied to all src projects
  config-test.toml   ← applied to all test projects
```

### Directory.Build.props Wiring

Both files are wired once at the solution root via `Directory.Build.props`. The analyzer receives whichever file matches the project type.

```xml
<ItemGroup>
  <AdditionalFiles
      Include="$(MSBuildThisFileDirectory).roslyn-lint\config-src.toml"
      Condition="Exists('$(MSBuildThisFileDirectory).roslyn-lint\config-src.toml')
                 AND '$(IsTestProject)' != 'true'" />
  <AdditionalFiles
      Include="$(MSBuildThisFileDirectory).roslyn-lint\config-test.toml"
      Condition="Exists('$(MSBuildThisFileDirectory).roslyn-lint\config-test.toml')
                 AND '$(IsTestProject)' == 'true'" />
</ItemGroup>
```

### Schema

Both files share the same schema. All `roslyn-lint` analyzers add their sections to these files — no additional `AdditionalFiles` wiring is needed when new analyzers are added.

```toml
# .roslyn-lint/config-src.toml  (or config-test.toml — same schema)

[dm001]
enabled = true
severity = "warning"           # "error" | "warning" | "info" | "hidden"
designated_file = "Constants.cs"
designated_class = "Constants" # optional; if omitted, only filename is checked

[dm002]
enabled = true
severity = "error"
forbidden = [
  "atm",          # exact match
  "atm*",         # prefix: any string starting with "atm"
  "*atm*",        # substring: any string containing "atm"
  "*atm",         # suffix: any string ending with "atm"
]
case_sensitive = false         # default false
```

---

## Rules

### DM001 — Constant Consolidation

**Diagnostic ID:** `DM001`  
**Category:** `roslyn-lint.Organization`  
**Default Severity:** Warning

#### Description

Public and internal `const` fields and `const` local declarations at the type level must be declared in the designated constants file/class as configured in `demagic.toml`. Constants found in any other file trigger this diagnostic.

#### Scope

- Applies to `const` fields with `public` or `internal` accessibility
- Applies at the type member level (not local constants inside method bodies)
- Does not apply to `static readonly` (v1)
- Does not apply to `private` or `protected` constants

#### Trigger Conditions

A violation is raised when:
- A `public` or `internal` `const` field is declared in a file whose name does not match `designated_file`
- If `designated_class` is also configured: the containing class name must also match

#### Diagnostic Message

```
DM001: Public/internal constant '{name}' should be declared in '{designated_file}'. 
Move to the designated constants file to consolidate constant definitions.
```

#### Example

```csharp
// ❌ Violation — ApiClient.cs contains a public const
public class ApiClient
{
    public const string DefaultEndpoint = "https://api.example.com"; // DM001
}

// ✅ Compliant — declared in Constants.cs
public static class Constants
{
    public const string DefaultEndpoint = "https://api.example.com";
}
```

---

### DM002 — Forbidden String Literals

**Diagnostic ID:** `DM002`  
**Category:** `roslyn-lint.DomainBoundary`  
**Default Severity:** Error

#### Description

String literals whose values match any pattern in the `forbidden` list are disallowed anywhere in the codebase. This rule enforces that domain-specific values do not appear in general-purpose code.

#### Scope

- Applies to all string literal expressions (`SyntaxKind.StringLiteralExpression`)
- Applies regardless of context: const declarations, comparisons, method arguments, attribute arguments, switch cases, return statements, etc.
- Applies to verbatim string literals (`@"..."`)
- Does not apply to interpolated string holes (v1)
- Does not apply to comments or XML documentation

#### Pattern Matching

Patterns in the `forbidden` list support three wildcard forms:

| Pattern | Meaning | Example | Matches |
|---|---|---|---|
| `"atm"` | Exact match | `"atm"` | `"atm"` only |
| `"atm*"` | Prefix match | `"atm*"` | `"atm"`, `"atm-core"`, `"atmDaemon"` |
| `"*atm*"` | Substring match | `"*atm*"` | `"atm"`, `"my-atm-tool"`, `"latm"` |
| `"*atm"` | Suffix match | `"*atm"` | `"atm"`, `"super-atm"` |

Matching is case-insensitive by default (`case_sensitive = false`).

#### Diagnostic Message

```
DM002: String literal '{value}' matches forbidden pattern '{pattern}'. 
This value suggests domain-specific coupling and is not permitted in this codebase.
```

#### Examples

```csharp
// Config: forbidden = ["atm", "atm*", "*atm*"]

// ❌ Exact match
public const string ToolName = "atm";                    // DM002 — matches "atm"

// ❌ Prefix match
public const string DaemonName = "atm-daemon";           // DM002 — matches "atm*"

// ❌ Substring match in expression
if (tool.Name == "my-atm-core") { ... }                  // DM002 — matches "*atm*"

// ❌ In method argument
logger.Log("starting atm process");                      // DM002 — matches "*atm*"

// ✅ No match
public const string ToolName = "agent-mail";             // OK
```

---

## NuGet Package

| Property | Value |
|---|---|
| Package ID | `Roslyn.DeMagic` |
| Assembly | `Roslyn.DeMagic.dll` |
| Target Frameworks | `netstandard2.0` (analyzer assembly) |
| Roslyn Dependency | `Microsoft.CodeAnalysis.CSharp` ≥ 4.0 |
| Package Type | `Analyzers` |
| Development Dependency | `true` |

The package ships a `.props` file that is a no-op — `AdditionalFiles` wiring is handled once by the consuming solution's `Directory.Build.props` (see Configuration). No per-project setup is required.

---

## Implementation Notes

### Config Loading

The TOML config is loaded via `AdditionalFiles`, wired in the solution's `Directory.Build.props`. The analyzer receives exactly one config file per project — `config-src.toml` or `config-test.toml` — and loads it directly with no merge logic. If no config file is present the analyzer disables all rules gracefully.

### Pattern Compilation

Forbidden patterns are compiled once at analyzer initialization into a `PatternMatcher` struct. Each pattern is classified by type (exact / prefix / suffix / substring) and stored for O(1) classification dispatch per literal.

### `roslyn-diff` / `roslyn-graph` Integration

`roslyn-diff` and `roslyn-graph` are candidates for absorption as supporting infrastructure once `roslyn-lint` matures. DM001 in particular could leverage graph-based constant reference analysis in a future version for smarter fix suggestions.

---

## Acceptance Criteria

### DM001
- [ ] Public `const` field outside designated file raises DM001
- [ ] Internal `const` field outside designated file raises DM001
- [ ] Private `const` field does not raise DM001
- [ ] Local `const` inside a method body does not raise DM001
- [ ] `const` in the designated file does not raise DM001
- [ ] Rule is suppressible via `#pragma warning disable DM001`
- [ ] Severity is respected from config

### DM002
- [ ] Exact match raises DM002
- [ ] Prefix wildcard (`atm*`) raises DM002 for matching literals
- [ ] Suffix wildcard (`*atm`) raises DM002 for matching literals
- [ ] Substring wildcard (`*atm*`) raises DM002 for matching literals
- [ ] Non-matching literals do not raise DM002
- [ ] Case-insensitive matching is the default
- [ ] `case_sensitive = true` config is respected
- [ ] Violation in const declaration raises DM002
- [ ] Violation in comparison expression raises DM002
- [ ] Violation in method argument raises DM002
- [ ] Rule is suppressible via `#pragma warning disable DM002`
- [ ] Severity is respected from config

### Config
- [ ] `config-src.toml` is loaded for non-test projects
- [ ] `config-test.toml` is loaded for test projects
- [ ] Missing config file disables all rules gracefully (no crash)
- [ ] Malformed TOML logs a diagnostic and disables the affected rule
- [ ] Each file is fully independent — no merging or inheritance between src and test configs

---

## Open Questions

| # | Question | Owner |
|---|---|---|
| 1 | Should DM001 also flag `static readonly` fields in v1 or defer to v2? | Rand |
| 2 | Code fix providers (move const, suppress literal) — v1 or v2? | Rand |
| 3 | Should interpolated string holes be analyzed for DM002 in v1? | Rand |

---

## Version History

| Version | Date | Notes |
|---|---|---|
| 0.1 | 2026-05-15 | Initial draft |
