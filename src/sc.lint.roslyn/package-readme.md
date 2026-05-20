# sc-lint-roslyn

`sc-lint-roslyn` is the shipped .NET tool package for the Roslyn lint suite.

## Install

```bash
dotnet tool install --global sc-lint-roslyn --version 0.2.0
```

## Command

```bash
sc-lint-roslyn version --json
```

## Tool Targets

- packaged tool targets: `net8.0` and `net10.0`
- runtime expectation: run the tool with a compatible `dotnet` host that can
  execute one of the shipped tool targets

## Current Command Families

- `lint`
- `view`
- `check`
- `clippy`
- `ci`
- `version`

## Repository

- project URL: `https://github.com/randlee/roslyn-lint`
- docs: `docs/sc-lint-roslyn/install.md`
