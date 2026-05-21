# sc-lint-roslyn-demagic Package Usage

## Package Id

- `sc-lint-roslyn-demagic`

## Package Reference

```xml
<ItemGroup>
  <PackageReference Include="sc-lint-roslyn-demagic" Version="0.2.0" />
</ItemGroup>
```

## Shipped Target Framework

- analyzer package target: `netstandard2.0`

The analyzer assembly is packaged as `netstandard2.0` for Roslyn host
compatibility. Consumer projects are expected to be SDK-style C# projects that
run on a Roslyn compiler host.

## Repository Configuration

The analyzer reads `.sc-lint-roslyn` configuration from the repository root.
The current shipped config shape is TOML-based and includes:

- `dm001.enabled`
- `dm001.severity`
- `dm001.designated_file`
- `dm001.designated_class`
- `dm002.enabled`
- `dm002.severity`
- `dm002.forbidden`
- `dm002.case_sensitive`

Example:

```toml
[dm001]
enabled = true
severity = "warning"
designated_file = "DeMagicConstants.cs"
designated_class = "DeMagicConstants"

[dm002]
enabled = true
severity = "warning"
forbidden = [
  "demagic",
  "sc-lint-roslyn",
]
case_sensitive = false
```

## Expected Behavior

- `DM001`
  - reports shared public and internal constants that should be consolidated
    into the designated constants file/class
- `DM002`
  - reports configured forbidden string literals

## Package Surface Notes

- package readme in the shipped `.nupkg`: `src/sc.lint.roslyn.demagic/package-readme.md`
- repository URL: `https://github.com/randlee/roslyn-lint`
- package project URL: `https://github.com/randlee/roslyn-lint`
