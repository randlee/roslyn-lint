# roslyn-lint Architecture

## 1. Overview

The `roslyn-lint` suite consists of:
- `Roslyn.DeMagic`, a Roslyn analyzer package
- `roslyn-lint`, a companion CLI distributed as a .NET tool
- test projects that verify analyzer and CLI behavior

The architecture must treat the current implementation as provisional. If the
existing code conflicts with this architecture or the product requirements, the
current code is expected to change or be removed.

## 1.1 Documentation Structure

This file owns suite-level architecture. Project-local architecture detail is
owned by:
- [`docs/roslyn-demagic/architecture.md`](./roslyn-demagic/architecture.md)
- [`docs/roslyn-lint/architecture.md`](./roslyn-lint/architecture.md)

Project-level requirements are owned by:
- [`docs/roslyn-demagic/requirements.md`](./roslyn-demagic/requirements.md)
- [`docs/roslyn-lint/requirements.md`](./roslyn-lint/requirements.md)

Planning and sequencing are owned by:
- [`docs/project-plan.md`](./project-plan.md)
- [`docs/phase-A/plan-phase-A.md`](./phase-A/plan-phase-A.md)

## 2. Project Boundaries

The repository currently contains two production projects:
- `src/Roslyn.DeMagic`
- `src/Roslyn.Lint`

Test coverage is owned by:
- `tests/Roslyn.DeMagic.Tests`
- `tests/Roslyn.Lint.Tests`

Boundary rules:
- `Roslyn.DeMagic` owns diagnostic rule semantics, configuration loading,
  diagnostic descriptors, and analyzer packaging.
- `roslyn-lint` owns command parsing, target resolution, execution
  orchestration, and output rendering for the CLI surface.
- test projects own behavior verification only; they must not become hidden
  production dependency points.
- repository-level build, package versions, and release metadata are shared
  through `Directory.Build.props` and the GitHub workflows.

## 3. System Shape

### 3.1 Shared Repository Layer

Repository-wide concerns are centralized at the root:
- `Directory.Build.props` for common .NET, analyzer, version, and package settings
- `.github/workflows/ci.yml` for cross-platform validation
- `.github/workflows/publish.yml` for tag-driven packaging and publication
- `roslyn-lint.sln` for solution composition

Architectural rules:
- common package versions and build behavior have one source of truth
- configuration file selection for analyzers is wired once at the solution
  root, not repeated ad hoc per project
- release automation validates both deliverables on the same release line

### 3.2 Analyzer Layer: `Roslyn.DeMagic`

`Roslyn.DeMagic` is the authoritative implementation of:
- `DM001` constant consolidation
- `DM002` forbidden string literal detection

Architectural rules:
- analyzer behavior is config-driven through `AdditionalFiles`
- the analyzer package must remain Roslyn-host compatible
- rule semantics must live inside the analyzer project, not inside the CLI
- diagnostic IDs, categories, default severities, and messages are part of the
  analyzer contract

### 3.3 CLI Layer: `roslyn-lint`

`roslyn-lint` is a companion executable that exposes the linting surface to
end users and automation.

Architectural rules:
- the CLI must stay thin
- the CLI may orchestrate analyzer execution, but it must not define alternate
  rule semantics for `DM001` or `DM002`
- output formatting, target resolution, and user interaction belong in the CLI
- analyzer packaging and Roslyn host behavior do not belong in the CLI layer

### 3.4 Test Layer

Tests are split by project ownership:
- analyzer tests validate documented rule behavior and exclusions
- CLI tests validate settings, command contracts, and output/exit semantics

## 4. Current Drift That Phase A Must Resolve

The current repository contains useful spike code, but it is not yet the
architectural baseline.

Known drift to resolve:
- current `DM001` and `DM002` implementations are simple magic-value analyzers
  and do not yet implement the config-driven PRD contract
- current analyzer categories and default severities do not match the PRD
  target
- current CLI execution is file-oriented and performs ad hoc compilation; it
  does not yet prove the full project-aware configuration contract
- current documentation does not yet fully explain project ownership,
  sequencing, or deletion criteria for conflicting code

Phase A may keep, refactor, or replace any of this code as long as the result
matches the documented target.

## 5. Architectural Rules

- top-level product behavior belongs in `docs/requirements.md`
- project-local implementation ownership belongs in `docs/<project>/`
- a requirement must have one source of truth
- the analyzer owns rule semantics; the CLI owns user-facing orchestration
- repository build and release behavior must remain centralized
- code that conflicts with documented requirements is not protected by prior
  existence

## 6. Phase A Target Architecture

Phase A converges the repository on this shape:
- one repository-wide documentation framework
- one repo-level product contract and project plan
- one requirements/architecture pair per production project
- one phase-A plan with sprint-level execution docs
- one config-driven analyzer implementation line for `Roslyn.DeMagic`
- one thin companion CLI line that either accurately orchestrates analyzer
  execution or is reduced until it does

Project-local detail is further specified in:
- [`docs/roslyn-demagic/architecture.md`](./roslyn-demagic/architecture.md)
- [`docs/roslyn-lint/architecture.md`](./roslyn-lint/architecture.md)
