# B3 Package Documentation Findings

## Public Package Surfaces

- `sc-lint-roslyn-demagic`
- `sc-lint-roslyn`

## Row Contract

| Package | Severity | Surface | Problem | Expected correction | Closed in B3? |
| --- | --- | --- | --- | --- | --- |
| `sc-lint-roslyn-demagic` | `medium` | repository/project URLs | shared package metadata still points to `https://github.com/randlee/sc-lint-roslyn` instead of the real repository | change `RepositoryUrl` and `PackageProjectUrl` to `https://github.com/randlee/roslyn-lint` in the shared package metadata | yes |
| `sc-lint-roslyn-demagic` | `medium` | package readme surface | shipped package reuses the suite-level repository `README.md`, which does not tell a package consumer how to add the analyzer package or what target it ships for | ship a package-specific readme and align it with `docs/sc-lint-roslyn-demagic/package-usage.md` | yes |
| `sc-lint-roslyn-demagic` | `medium` | analyzer package-reference guidance | no canonical package-facing usage doc explains the package id, `PackageReference`, target, or `.sc-lint-roslyn` config expectation | expand `docs/sc-lint-roslyn-demagic/package-usage.md` into the canonical analyzer package-usage guide | yes |
| `sc-lint-roslyn-demagic` | `low` | target-framework disclosure | the analyzer target is visible in the project file, but not in the package-facing docs | publish `netstandard2.0` explicitly in package usage and package readme content | yes |
| `sc-lint-roslyn` | `medium` | package readme surface | shipped tool package reuses the suite-level repository `README.md`, which does not tell a tool consumer how to install or verify the tool | ship a package-specific readme and align it with `docs/sc-lint-roslyn/install.md` | yes |
| `sc-lint-roslyn` | `medium` | install guidance | no canonical install doc explains the `dotnet tool install` shape, verification command, or runtime expectations | expand `docs/sc-lint-roslyn/install.md` into the canonical tool install guide | yes |
| `sc-lint-roslyn` | `low` | target/runtime disclosure | the CLI targets are visible in `Directory.Build.props`, but not in package-facing docs | publish the tool targets (`net8.0`, `net10.0`) and compatible host expectation explicitly | yes |
| both shipped packages | `low` | package-surface contract boundary | package-facing expectations were implied across multiple files rather than explicitly named in one contract document | use `docs/phase-B/package-surface-contract.md` as the package-surface boundary doc for B3 | yes |

## B3 Closeout Summary

- no remaining metadata-only package-surface gaps were left open after B3
- package-facing guidance now exists for:
  - analyzer package reference
  - CLI install and verification
  - shipped package targets/runtime expectations
  - correct repository/project URLs

## Covered Surface Types

- package readme content
- package description/tags
- repository/project/package URLs
- analyzer package-reference guidance
- CLI install guidance
- target-framework/runtime disclosure
- release-note presentation
