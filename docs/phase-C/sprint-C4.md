---
id: C4
title: roslyn graph extraction
status: planned
branch: sprint/C4
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/C4
target: integration/phase-C
---

# Sprint C4 - Roslyn Graph Extraction

## Goal

- Deliver Roslyn graph extraction only.

## Hard Dependencies

- `docs/phase-C/sprint-C3.md`
- `docs/sc-lint-roslyn-boundary/requirements.md`
- `docs/sc-lint-roslyn-boundary/architecture.md`
- `docs/phase-C/graph-schema-blockers.md`

## Exact Targets

- `src/sc.lint.roslyn.boundary/Graph/`
- `src/sc.lint.roslyn.boundary/Graph/RoslynGraphExtractor.cs`
- `src/sc.lint.roslyn.boundary/Graph/GraphNode.cs`
- `src/sc.lint.roslyn.boundary/Graph/GraphEdge.cs`
- `src/sc.lint.roslyn.boundary/Graph/GraphModel.cs`
- `tests/sc.lint.roslyn.boundary.tests/Graph/`
- `docs/sc-lint-roslyn-boundary/architecture.md`

## Required Work

- extract the internal Roslyn-side graph model needed for later boundary
  analysis
- define node and edge categories needed for boundary parity and export
- document which Roslyn constructs are in initial extraction scope and which
  are not
- add tests for deterministic graph extraction on representative source inputs
- do not implement final export schema output or rule evaluation in this sprint

## Acceptance Criteria

- the internal graph model is documented and implemented
- graph extraction is deterministic for the in-scope Roslyn inputs
- the sprint does not claim final export-schema completion
- boundary rules are not yet mixed into this sprint

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
