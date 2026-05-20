# Phase B Package Surface Contract

## Purpose

Define the public package/documentation boundary Phase B must make consistent
for the shipped analyzer and CLI.

## Analyzer Package Surface

- package id: `sc-lint-roslyn-demagic`
- consumer action: add a `PackageReference`
- required published guidance:
  - package id
  - reference example
  - supported target frameworks
  - repository/project URL

## CLI Package Surface

- package id: `sc-lint-roslyn`
- consumer action: install as a `.NET` tool
- required published guidance:
  - install command
  - invocation command
  - runtime expectations / supported target frameworks
  - repository/project URL

## Canonical Metadata Fields

- `RepositoryUrl`
- `PackageProjectUrl`
- `PackageReadmeFile`
- `PackageReleaseNotes`
- `Description`
- `PackageTags`

## Current B3 Package Readme Surfaces

- analyzer package readme: `src/sc.lint.roslyn.demagic/package-readme.md`
- CLI tool package readme: `src/sc.lint.roslyn/package-readme.md`
