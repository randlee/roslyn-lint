# sc-lint-roslyn-demagic

`sc-lint-roslyn-demagic` is the shipped analyzer package for the Phase A/B
`DM001` and `DM002` rules.

## Package Id

```xml
<PackageReference Include="sc-lint-roslyn-demagic" Version="0.2.0" />
```

## Package Target

- analyzer assembly target: `netstandard2.0`

The analyzer package is intended for SDK-style C# projects that run on a Roslyn
compiler host. The analyzer assembly itself is `netstandard2.0` for Roslyn host
compatibility.

## Rules

- `DM001`
  - shared public and internal constants must be consolidated into the
    designated constants file/class
- `DM002`
  - configured forbidden string literals are reported from
    `.sc-lint-roslyn` configuration

## Basic Usage

1. Add the package reference to the target C# project.
2. Add `.sc-lint-roslyn` configuration in the repository root as needed.
3. Build the project normally.

## Repository

- project URL: `https://github.com/randlee/roslyn-lint`
- docs: `docs/sc-lint-roslyn-demagic/package-usage.md`
