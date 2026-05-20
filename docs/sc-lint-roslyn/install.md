# sc-lint-roslyn Installation

## Purpose

Phase B uses this document as the canonical install guide for the shipped
`sc-lint-roslyn` CLI package.

## Required Published Content

- package id: `sc-lint-roslyn`
- install example:

```bash
dotnet tool install --global sc-lint-roslyn --version 0.1.x
```

- invocation example:

```bash
sc-lint-roslyn version --json
```

- runtime expectations / supported target frameworks: must match the shipped
  CLI tool package target/runtime baseline
- repository/project URL: must point to the correct GitHub repository
