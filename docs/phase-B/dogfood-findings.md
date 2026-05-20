# B1 Analyzer Dogfood Findings

## Scope

- `src/sc.lint.roslyn.demagic`
- `src/sc.lint.roslyn.abstractions`
- `src/sc.lint.roslyn.demagic.lint`
- `src/sc.lint.roslyn`
- `tests/sc.lint.roslyn.demagic.tests`
- `tests/sc.lint.roslyn.tests`

## Row Contract

| Project | Rule | Severity | Location | Finding summary | Expected? | Disposition |
| --- | --- | --- | --- | --- | --- | --- |
| `src/sc.lint.roslyn.demagic` | `DM001` | `medium` | `src/sc.lint.roslyn.demagic/diagnostics/DeMagicDiagnosticDescriptors.cs:8,9,11,12,14,15,17,20,23,26`; `src/sc.lint.roslyn.demagic/analyzers/DM001ConstantConsolidationAnalyzer.cs:13`; `src/sc.lint.roslyn.demagic/analyzers/DM002ForbiddenStringLiteralAnalyzer.cs:15` | Warm-build self-analysis reports `12` public/internal constants that should be consolidated into a designated constants file instead of remaining distributed across analyzer and descriptor types. | `yes` | `immediate-fix` |
| `src/sc.lint.roslyn.abstractions` | `none` | `none` | `no findings` | No DM001 or DM002 diagnostics were emitted during the B1 build pass. The shared abstractions layer stayed clear under the initial source-project config. | `yes` | `no-action` |
| `src/sc.lint.roslyn.demagic.lint` | `DM002` | `low` | `src/sc.lint.roslyn.demagic.lint/RoslynDeMagicToolModule.cs:9` | The tool-module registration uses the product identifier string `demagic` inline. The analyzer flags it consistently in both target frameworks. | `mixed` | `later-phase-remediation` |
| `src/sc.lint.roslyn` | `DM002` | `medium` | `src/sc.lint.roslyn/commandmodel/LintProfileCatalog.cs:13,18,23`; `src/sc.lint.roslyn/commands/RegisterLintCommands.cs:25`; `src/sc.lint.roslyn/commands/RegisterVersionCommand.cs:17` | The CLI shell repeats the shipped tool id `demagic` and CLI id `sc-lint-roslyn` inline across command registration and profile metadata. The analyzer behavior is predictable and exposes real consolidation candidates in production code. | `yes` | `immediate-fix` |
| `tests/sc.lint.roslyn.demagic.tests` | `none` | `none` | `no findings` | No diagnostics were emitted from the compiled analyzer-test assembly. Fixture files under `testdata/` remain excluded from compilation, so B1 only evaluates the real test harness code here. | `yes` | `no-action` |
| `tests/sc.lint.roslyn.tests` | `DM002` | `low` | `tests/sc.lint.roslyn.tests/abstractions/ToolDescriptorTests.cs`; `commands/*.cs`; `contracts/*.cs`; `dispatch/*.cs`; `formatting/*.cs`; `operations/*.cs` | The test suite intentionally repeats `demagic` and `sc-lint-roslyn` when asserting shipped command and envelope surface. The analyzer flags those literals consistently across both test target frameworks, producing high-volume but predictable noise. | `mixed` | `later-phase-remediation` |

## Reproduction

Exact B1 warm-build self-analysis sequence:

1. `dotnet restore sc-lint-roslyn.sln`
2. `dotnet build sc-lint-roslyn.sln --configuration Release`
3. `dotnet build sc-lint-roslyn.sln --configuration Release`

The first build produces the analyzer output used for self-hosted analysis.
The second build is the first deterministic pass where
`src/sc.lint.roslyn.demagic` reports its own `DM001` findings under the local
dogfood wiring.

## Enforcement Mode

- B1 keeps `DM001` and `DM002` non-blocking by routing them through
  `WarningsNotAsErrors` during the discovery pass.
- The goal for this sprint is inventory and classification, not immediate
  merge-gating.

## Rules

- every in-scope project must have at least one explicit row or an explicit
  `no findings` note
- every row must map to `DM001`, `DM002`, or a clearly documented analyzer
  behavior gap
