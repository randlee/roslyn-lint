# B2 CLI Dogfood Findings

## Command Scope

- `sc-lint-roslyn version --json`
- `sc-lint-roslyn lint demagic --json`
- `sc-lint-roslyn view tools --json`
- `sc-lint-roslyn check --json`
- `sc-lint-roslyn clippy --json`
- `sc-lint-roslyn ci --json`

## Row Contract

| Command/workflow | Input shape | Observed behavior | Expected? | Disposition |
| --- | --- | --- | --- | --- |
| `version --json` | `dotnet run --no-build --project src/sc.lint.roslyn/sc.lint.roslyn.csproj --framework net10.0 -- version --json` | Returns a clean success envelope with `command: "version"` and `data.cli = "sc-lint-roslyn"`, `data.version = "0.1.2.0"` | yes | `immediate-fix` |
| `view tools --json` | `dotnet run --no-build --project src/sc.lint.roslyn/sc.lint.roslyn.csproj --framework net10.0 -- view tools --json` | Returns a clean success envelope with a single registered tool id `demagic` and stable `commandFamilies` / `capabilities` metadata | yes | `immediate-fix` |
| `lint demagic --json` | `dotnet run --no-build --project src/sc.lint.roslyn/sc.lint.roslyn.csproj --framework net10.0 -- lint demagic --json` from repo root | Returns a typed success envelope, but the default target path is the current working directory and the scan includes `src/`, `tests/`, `examples/`, and `tests/*/testdata/`; the finding set is therefore broader than the B1 in-scope project set and includes package-smoke samples and analyzer test data | no | `contract-clarification` |
| `check --json` | `dotnet run --no-build --project src/sc.lint.roslyn/sc.lint.roslyn.csproj --framework net10.0 -- check --json` | Returns a clean typed failure envelope with `CLI.BACKEND_EXEC_FAILURE` because the wrapped `dotnet build sc-lint-roslyn.sln --configuration Release --no-restore` step fails under current dogfood warnings | yes | `immediate-fix` |
| `clippy --json` | `dotnet run --no-build --project src/sc.lint.roslyn/sc.lint.roslyn.csproj --framework net10.0 -- clippy --json` | Returns a clean typed failure envelope with `CLI.BACKEND_EXEC_FAILURE` because the wrapped `dotnet build ... -warnaserror` step fails under current dogfood warnings | yes | `immediate-fix` |
| `ci --json` | `dotnet run --no-build --project src/sc.lint.roslyn/sc.lint.roslyn.csproj --framework net10.0 -- ci --json` | Returns a clean typed failure envelope with `CLI.CI_LINT_FAILURE` and `finding_count = 74`; this is predictable given the current repo state and the default lint target scope | yes | `immediate-fix` |
| local `dotnet run ... --json` transport | `dotnet run --project src/sc.lint.roslyn/sc.lint.roslyn.csproj --framework net10.0 -- <command> --json` without `--no-build` | JSON envelopes are prefixed by Roslyn build warnings when the implicit build emits output, so machine mode is not transport-clean for callers that rely on `dotnet run` rather than a prebuilt binary or `--no-build` | no | `contract-clarification` |
| concurrent local `dotnet run` transport | multiple `dotnet run --project src/sc.lint.roslyn/sc.lint.roslyn.csproj ...` commands launched in parallel in the same worktree | concurrent runs can race on intermediate analyzer outputs and surface `CS2012` file-lock failures during the build phase | yes | `later-phase-remediation` |

## Summary

- clean machine-mode contract behavior was confirmed for `version`, `view tools`,
  `check`, `clippy`, and `ci` when the CLI is executed through a prebuilt binary
  or `dotnet run --no-build`
- the main predictability gap exposed by B2 is not the JSON envelope itself; it
  is the difference between the CLI process contract and the build-time output
  added by plain `dotnet run`
- the main scope ambiguity exposed by B2 is that `lint demagic` currently uses
  the current working directory as its target root, which makes repo-root
  dogfooding include package-smoke samples and analyzer testdata unless callers
  intentionally invoke the CLI from a narrower root
