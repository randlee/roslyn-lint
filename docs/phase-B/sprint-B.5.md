---
id: B.5
title: source-only consolidation cleanup
status: complete
branch: sprint/B.5
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/B.5
target: integration/phase-B
---

# Sprint B.5 - Source-Only Consolidation Cleanup

## Goal

- apply the Phase B analyzer findings directly in production code
- consolidate repeated semantic strings and shared constants into owned
  `src/` sources
- remove analyzer dogfooding from `tests/` so this remediation line targets
  production code only

## Hard Dependencies

- `docs/phase-B/plan-phase-B.md`
- `docs/phase-B/sprint-B1.md`
- `docs/phase-B/dogfood-findings.md`
- `docs/phase-B/dogfood-remediation-policy.md`
- `Directory.Build.props`
- `src/sc.lint.roslyn.demagic/`
- `src/sc.lint.roslyn.abstractions/`
- `src/sc.lint.roslyn.demagic.lint/`
- `src/sc.lint.roslyn/`

## Exact Targets

- `docs/phase-B/sprint-B.5.md`
- `docs/phase-B/plan-phase-B.md`
- `docs/project-plan.md`
- `Directory.Build.props`
- `src/sc.lint.roslyn.demagic/DeMagicConstants.cs`
- `src/sc.lint.roslyn.demagic/diagnostics/DeMagicDiagnosticDescriptors.cs`
- `src/sc.lint.roslyn.demagic/analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/sc.lint.roslyn.demagic/analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/sc.lint.roslyn.abstractions/ScLintRoslynConstants.cs`
- `src/sc.lint.roslyn.demagic.lint/RoslynDeMagicToolModule.cs`
- `src/sc.lint.roslyn.demagic.lint/DeMagicWorkspaceAdapter.cs`
- `src/sc.lint.roslyn/commandmodel/LintProfileCatalog.cs`
- `src/sc.lint.roslyn/commands/`
- `src/sc.lint.roslyn/CliApplication.cs`
- `src/sc.lint.roslyn/operations/`
- `tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj`
- `tests/sc.lint.roslyn.tests/sc.lint.roslyn.tests.csproj`

## Deliverables

- repeated semantic strings in `src/` have one owned source
- shared production constants are consolidated into a small number of owned
  constants files
- `DM001` production constants are moved into the designated
  `DeMagicConstants.cs` owner
- analyzer dogfooding is limited to `src/` projects only
- the integration line validates cleanly after consolidation

## Required Work

- create a designated constants owner in `src/sc.lint.roslyn.demagic/` and
  move shared DM001/DM002 descriptor strings there
- replace scattered production literals for tool ids, CLI ids, command ids,
  and solution/config names with shared owned constants under `src/`
- keep incidental one-off prose inline when it is not a shared semantic value
  or branching key
- remove analyzer dogfooding from `tests/` projects and keep the dogfood scope
  production-only for this cleanup line
- ensure the post-consolidation source build reduces the Phase B source
  findings relative to the original B1/B2 production baseline

## Non-Closure Items

- B.5 does not reintroduce analyzer dogfooding for `tests/`
- B.5 does not change Phase C boundary-package scope
- B.5 does not require broad prose-string extraction unrelated to shared
  semantic identifiers

## Acceptance Criteria

- each repeated semantic string in `src/` has one canonical owned source
- shared production constants are consolidated into a small set of owned
  constants files
- `Directory.Build.props` scopes analyzer dogfooding to `src/` only
- test projects explicitly do not participate in this analyzer dogfooding line
- `dotnet restore`, `dotnet build`, `dotnet test`, and `git diff --check`
  all pass
- post-consolidation source findings are reduced from the original B1/B2
  source baseline

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
