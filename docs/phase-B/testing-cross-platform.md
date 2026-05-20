# Phase B Testing And Cross-Platform Guidance

## Purpose

Capture the validation expectations for local dogfooding and package-surface
cleanup when the resulting guidance must work across developer environments.

## Guidance

- `B1` and `B2` validate through normal solution restore/build/test on the
  local repository
- `B3` must validate package metadata and documentation against the packed
  analyzer and CLI packages
- `B3` install guidance must avoid platform-specific assumptions unless the
  doc states them explicitly

## Required Validation Baseline

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `dotnet pack src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj --configuration Release -o artifacts/packages`
- `dotnet pack src/sc.lint.roslyn/sc.lint.roslyn.csproj --configuration Release -o artifacts/packages`
- `git diff --check`
