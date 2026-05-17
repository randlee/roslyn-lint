# roslyn-lint CLI Architecture

## 1. Purpose

This document defines the `roslyn-lint` CLI project architecture.

It complements the suite-level architecture in
[`../architecture.md`](../architecture.md) and owns CLI-local structure.

## 2. Architectural Rules

- the CLI is a thin companion surface
- rule semantics belong to `Roslyn.DeMagic`, not to the CLI
- output formatting and command interaction belong to the CLI
- unsupported or misleading execution paths must be removed rather than left in
  place as accidental product behavior

## 3. Target Component Model

### 3.1 Entry Layer

Owns:
- `Program.cs`
- command application startup
- version and application metadata

### 3.2 Command Layer

Owns:
- command registration
- settings validation
- argument parsing
- command lifecycle

### 3.3 Analysis Orchestration Layer

Owns:
- target file or project discovery
- invocation of analyzer execution
- collection of diagnostics for presentation

This layer must not redefine analyzer rule behavior.

### 3.4 Presentation Layer

Owns:
- text rendering
- JSON rendering
- exit code shaping

## 4. Phase A Architectural Direction

The current CLI spike directly parses source files into ad hoc compilations and
executes the analyzer instances locally. That may be a useful bootstrap path,
but it is not automatically the correct product architecture.

Phase A architectural direction:
- keep the CLI only if it can faithfully represent the documented analyzer
  contract
- if project-aware or config-aware execution requires a different orchestration
  path, refactor the CLI to that shape
- if a feature cannot be made faithful in Phase A, narrow or remove that path
  rather than shipping a misleading tool

## 5. Packaging Boundary

The CLI owns .NET tool packaging and command-name identity. It must not absorb
analyzer package responsibilities or cross-project diagnostic metadata.
