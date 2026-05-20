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
- `docs/phase-B/package-surface-contract.md`
- `docs/phase-B/testing-cross-platform.md`
- `docs/sc-lint-roslyn-demagic/package-usage.md`
- `docs/sc-lint-roslyn/install.md`
- `src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj`
- `src/sc.lint.roslyn/sc.lint.roslyn.csproj`
- `Directory.Build.props`

## Deliverables

- `docs/phase-B/package-doc-findings.md` inventories every public package-
  surface gap independently
- `docs/phase-B/package-doc-follow-up-issues.md` records every remaining
  package-surface gap that requires later product work independently
- `docs/sc-lint-roslyn-demagic/package-usage.md` provides analyzer package-
  reference guidance and target-framework support
- `docs/sc-lint-roslyn/install.md` provides CLI install guidance and runtime
  expectations
- `docs/phase-B/package-surface-contract.md` defines the public package
  metadata, package-reference, install, and target-framework disclosure
  boundary
- `docs/phase-B/testing-cross-platform.md` captures the validation guidance for
  package/install verification
- shipped package metadata points to the correct repository/project URLs
- shipped package metadata and public readme surfaces match actual shipped
  behavior

## Important Interfaces, Records/Structs, And Enums

Important package-reference and install signatures:

```xml
<ItemGroup>
  <PackageReference Include="sc-lint-roslyn-demagic" Version="$(Version)" />
</ItemGroup>
```

```bash
dotnet tool install --global sc-lint-roslyn --version 0.1.x
```

```xml
<PropertyGroup>
  <RepositoryUrl>https://github.com/randlee/roslyn-lint</RepositoryUrl>
  <PackageProjectUrl>https://github.com/randlee/roslyn-lint</PackageProjectUrl>
</PropertyGroup>
```

```text
Analyzer package target: netstandard2.0
CLI tool targets: documented from the shipped tool project/runtime baseline
```

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
- every listed deliverable in this sprint is expected to land at a
  production-ready level for B3's claimed scope; no deliverable may close in a
  shape-only, placeholder, or “document later” state

## Non-Closure Items

- B3 does not add new analyzer rules or new CLI command families
- B3 does not introduce the boundary package
- B3 may file follow-up issues for documentation gaps that require later
  product implementation rather than metadata cleanup alone

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
- the published package-facing docs disclose analyzer package-reference
  guidance, CLI install guidance, and target-framework/runtime support

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet pack src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj --configuration Release -o artifacts/packages`
- `dotnet pack src/sc.lint.roslyn/sc.lint.roslyn.csproj --configuration Release -o artifacts/packages`
- `git diff --check`
