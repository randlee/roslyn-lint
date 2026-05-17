---
id: A3
title: DM002 and analyzer hardening
status: planned
branch: integration/phase-A
target: Roslyn.DeMagic
---

# Sprint A3 - DM002 And Analyzer Hardening

## Goal

- Implement the forbidden-string rule from the PRD and harden the analyzer
  configuration and validation path.
- Replace the current generic string-literal spike with compiled forbidden-
  pattern analysis.

## Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-demagic/boundaries.md`
- `docs/phase-A/sprint-A2.md`

## Exact Targets

- `src/Roslyn.DeMagic/Analyzers/MagicStringAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/Roslyn.DeMagic/Patterns/IForbiddenPatternCompiler.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPattern.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPatternKind.cs`
- `src/Roslyn.DeMagic/Patterns/CompiledForbiddenPattern.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPatternMatcher.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfig.cs`
- `src/Roslyn.DeMagic/Diagnostics/DeMagicDiagnosticDescriptors.cs`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `tests/Roslyn.DeMagic.Tests/Analyzers/MagicStringAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/Analyzers/DM002ForbiddenStringLiteralAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/Patterns/ForbiddenPatternMatcherTests.cs`
- `tests/Roslyn.DeMagic.Tests/Patterns/ForbiddenPatternCompilerTests.cs`

## Important Interfaces, Records/Structs, and Enums

- interfaces:
  `IForbiddenPatternCompiler`
- immutable pattern payload types:
  `ForbiddenPattern`, `CompiledForbiddenPattern`
- enums:
  `ForbiddenPatternKind`, `ConfiguredSeverity`

## Required Work

- delete or retire the current `MagicStringAnalyzer` spike if it obstructs the
  approved `DM002` design
- extend the immutable config model for `dm002`
- implement exact, prefix, suffix, and substring forbidden-pattern matching
- support case sensitivity options
- validate graceful disablement, invalid-config, and severity behavior
- align analyzer release metadata with the actual rule set and diagnostic
  categories
- ensure the pattern compiler and matcher boundaries are testable without
  analyzer callback registration

## Acceptance Criteria

- `DM002` behavior matches the PRD
- pattern matching is covered by positive and negative tests
- exact, prefix, suffix, and substring matching semantics are represented by a
  closed enum and explicit payload types
- release metadata and descriptor categories match the approved diagnostics
- spike-era generic magic-string semantics are no longer part of the product

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
