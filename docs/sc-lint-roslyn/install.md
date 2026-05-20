# sc-lint-roslyn Installation

## Package Id

- `sc-lint-roslyn`

## Install

```bash
dotnet tool install --global sc-lint-roslyn --version 0.2.0
```

## Verify Install

```bash
sc-lint-roslyn version --json
```

## Shipped Tool Targets

- packaged tool targets: `net8.0` and `net10.0`
- runtime expectation: a compatible `dotnet` host must be available locally to
  execute one of the packaged tool targets

## Current Command Families

- `lint`
- `view`
- `check`
- `clippy`
- `ci`
- `version`

## Machine Mode Note

The stable machine-readable contract applies to the CLI process output itself.
For local contract verification, prefer a prebuilt binary or:

```bash
dotnet run --no-build --project src/sc.lint.roslyn/sc.lint.roslyn.csproj --framework net10.0 -- version --json
```

Plain `dotnet run` may prepend build warnings before the CLI process starts.

## Package Surface Notes

- shipped tool command name: `sc-lint-roslyn`
- package readme in the shipped `.nupkg`: `src/sc.lint.roslyn/package-readme.md`
- repository URL: `https://github.com/randlee/roslyn-lint`
- package project URL: `https://github.com/randlee/roslyn-lint`
