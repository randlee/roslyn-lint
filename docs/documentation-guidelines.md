# Roslyn Lint Documentation Guidelines

## 1. Purpose

This document defines how `roslyn-lint` suite and project documentation is
organized.

The goals are:

- one clear source of truth for each requirement or architectural decision
- explicit ownership boundaries between suite documentation and project
  documentation
- no duplicated requirement text across files
- traceability from product behavior to project implementation responsibility
- clear documentation support for deleting and replacing noncompliant spike
  code

## 2. Principles

### 2.1 One Requirement, One Home

A requirement must be written in exactly one place.

- suite-level behavior belongs in top-level requirements
- project-level implementation obligations belong in project requirements
- a document may reference a requirement owned elsewhere, but it must not copy
  the same requirement text as a second source of truth

### 2.2 Top-Level Docs Define Suite Behavior

Files in `docs/` at the top level define:

- suite product scope
- suite architecture
- implementation phases
- cross-project behavior

Top-level docs must not drift into project-local implementation detail unless
that detail is necessary to explain a suite-level decision.

### 2.3 Project Docs Define Ownership

Files in `docs/roslyn-demagic/` and `docs/roslyn-lint/` define:

- what each project owns
- how each project satisfies referenced suite requirements
- project-local boundaries
- project-local architectural decisions
- stable machine-contract detail when a project exposes a transport surface

Project docs must not redefine the suite contract.

### 2.4 Traceability Over Duplication

When a suite requirement is implemented by one or more projects:

- the suite requirement points to the owning project docs
- the project docs reference the suite requirement family they satisfy

The goal is to make missing ownership and overlapping ownership obvious.

### 2.5 Boundary Leaks Must Be Obvious

The documentation structure should make these failures easy to detect:

- CLI docs owning analyzer rule logic
- analyzer docs owning CLI transport or presentation concerns
- product behavior duplicated in multiple files
- multiple projects claiming the same implementation responsibility

### 2.6 Noncompliant Spikes Are Disposable

Current code and current documents are subordinate to approved requirements and
architecture.

If a spike implementation does not comply:

- removal and replacement is preferred to preserving unapproved structure
- compatibility with the spike is not a requirement by default
- planning documents should say so explicitly when a current implementation is
  disposable

## 3. Directory Layout

The required documentation layout is:

```text
docs/
  documentation-guidelines.md
  requirements.md
  architecture.md
  project-plan.md
  adr/
    INDEX.md
    ADR-001-analyzer-first-replacement-policy.md
    ADR-002-demagic-additionalfiles-config.md
    ADR-003-ai-cli-json-contract.md
    ADR-004-roslyn-lint-command-surface-and-parser.md
  prd/
    roslyn-demagic-prd.md
  phase-A/
    plan-phase-A.md
    sprint-A1.md
    sprint-A2.md
    sprint-A3.md
    sprint-A4.md
  roslyn-demagic/
    requirements.md
    architecture.md
    boundaries.md
  roslyn-lint/
    requirements.md
    architecture.md
    cli-contract.md
    boundaries.md
```

Notes:

- top-level docs remain the suite source of truth
- project-level docs own project-specific boundaries and implementation detail
- phase docs own sequencing and acceptance gates
- PRDs are input documents, not replacements for approved requirements and
  architecture

## 4. Top-Level Document Responsibilities

### 4.1 `docs/requirements.md`

Owns:

- suite product definition
- suite scope
- cross-project behavioral requirements
- Phase A priorities

Must not own:

- project-local class or module layouts
- project-local implementation details beyond what is needed to define behavior

### 4.2 `docs/architecture.md`

Owns:

- suite shape
- project boundaries
- cross-cutting architectural rules
- shared contract direction

Must not duplicate full project-local boundary inventories.

### 4.3 `docs/project-plan.md`

Owns:

- work phases
- sequencing
- milestones
- acceptance gates
- implementation replacement rules for noncompliant spikes

It should reference requirements and architecture rather than restating them in
full.

### 4.4 `docs/adr/`

Owns:

- accepted cross-cutting architectural decisions that are intended to remain
  enforceable
- decision context, drivers, consequences, and follow-up obligations

ADRs must not duplicate whole requirements documents, but they must record the
decisions that implementation and review are expected to enforce.

### 4.5 `docs/phase-A/`

Owns:

- Phase A sprint sequencing
- acceptance gates
- sprint-local implementation focus

Phase docs must not invent behavior that conflicts with top-level or
project-level requirements.

Sprint plan files must use the standard planning shape:

- frontmatter with `id`, `title`, `status`, `branch`, and `target`
- explicit hard dependencies
- explicit exact targets
- explicit required validation commands

## 5. Project-Level Document Responsibilities

### 5.1 `docs/roslyn-demagic/`

Owns analyzer-specific documentation:

- analyzer requirements
- analyzer architecture
- analyzer boundary inventory
- configuration and rule-ownership detail

It must not own CLI transport, formatting, or MCP wrapper concerns.

### 5.2 `docs/roslyn-lint/`

Owns CLI-specific documentation:

- CLI requirements
- CLI architecture
- CLI contract and command-family invariants
- CLI boundary inventory
- machine-contract and operation-layer ownership detail

It must not own analyzer rule semantics beyond referencing the analyzer project.

## 6. Boundary Documents

Each project boundary document exists to make ownership and future review
clearer.

Boundary docs should define:

- named owned boundaries
- each boundary's purpose
- boundary-specific notes about what is intentionally excluded

Boundary docs should not become unstructured design dumps.

## 7. Change Rules

- when a requirement moves, delete the superseded text from the old location
- when a spike is rejected, update the docs to say so explicitly
- when implementation diverges from approved docs, fix the implementation or
  replace it; do not silently relax the docs to match the spike
- when a sprint plan is too vague to drive code changes directly, rewrite it
  before implementation starts
