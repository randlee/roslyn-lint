---
id: B3
title: published package documentation cleanup
status: planned
branch: sprint/B3
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/B3
target: integration/phase-B
---

# Sprint B3 - Published Package Documentation Cleanup

## Goal

- Clean up the public package surface for the shipped analyzer and CLI.
- Make the NuGet/package presentation accurate, complete, and aligned with the
  real product state.
- Remove missing or misleading package-surface information before later phases
  broaden the product set.

## Hard Dependencies

- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `docs/phase-B/sprint-B1.md`
- `docs/phase-B/sprint-B2.md`
- `README.md`
- `docs/releasing.md`
- `src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj`
- `src/sc.lint.roslyn/sc.lint.roslyn.csproj`

## Exact Targets

- `README.md`
- `docs/releasing.md`
- `docs/phase-B/package-doc-findings.md`
- `docs/phase-B/package-doc-follow-up-issues.md`
- `docs/sc-lint-roslyn-demagic/package-usage.md`
- `docs/sc-lint-roslyn/install.md`
- `src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj`
- `src/sc.lint.roslyn/sc.lint.roslyn.csproj`
- `Directory.Build.props`

## Required Work

- inventory the current public package-surface gaps for the shipped analyzer
  and CLI, including readme content, package descriptions, tags, release-note
  presentation, linked documentation expectations, and repository/project URL
  correctness
- record every discovered package-surface gap in
  `docs/phase-B/package-doc-findings.md`
- update the shipped package metadata and documentation surfaces so they match
  the actual shipped product state
- correct repository/project/package URLs anywhere the shipped package metadata
  points to the wrong GitHub location
- add explicit analyzer package-reference instructions for normal consumer
  projects, including the expected package id and supported target frameworks
- add explicit CLI install instructions for `sc-lint-roslyn`, including the
  installation command shape and supported target frameworks/runtime
  expectations
- publish the .NET target-framework support for both shipped packages in the
  package-facing documentation, not only in project files
- create `docs/phase-B/package-doc-follow-up-issues.md` for anything that
  cannot be fully corrected in B3 without new product work
- verify that the shipped analyzer and CLI packages present a coherent
  user-facing story on package feeds and release docs

## Acceptance Criteria

- `docs/phase-B/package-doc-findings.md` exists and inventories the package
  documentation gaps B3 addressed
- package readme and metadata surfaces for both shipped packages are updated
  to match actual product behavior
- analyzer package-reference instructions exist and identify the supported
  target frameworks for `sc.lint.roslyn.demagic`
- CLI install instructions exist and identify the supported target
  frameworks/runtime expectations for `sc-lint-roslyn`
- `docs/phase-B/package-doc-follow-up-issues.md` exists for any remaining
  public-package gaps that require later implementation work
- no known misleading or obviously missing public package metadata remains in
  the shipped analyzer and CLI package definitions
- repository/project/package URLs in shipped package metadata point to the
  correct GitHub repository

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet pack src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj --configuration Release -o artifacts/packages`
- `dotnet pack src/sc.lint.roslyn/sc.lint.roslyn.csproj --configuration Release -o artifacts/packages`
- `git diff --check`
