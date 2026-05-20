# B2 CLI Dogfood Follow-Up Issues

## Row Contract

- use one row per unresolved CLI predictability, ownership, or contract issue

## Rule

- every CLI predictability gap, contract surprise, ownership ambiguity, or
  usability defect that is not closed inside `B2` must have an entry here

| Id | Command/workflow | Problem | Required follow-up | Owner phase/sprint |
| --- | --- | --- | --- | --- |
| `B2-CLI-001` | plain `dotnet run ... --json` | implicit build output can prefix the JSON envelope, so local wrapper invocation is not machine-clean by default | keep `docs/sc-lint-roslyn/cli-contract.md` explicit that the stable machine contract applies to the CLI process output itself; consider later developer ergonomics work only if a repo-local wrapper is added | `B2` |
| `B2-CLI-002` | `lint demagic --json` from repo root | default target scope is the current working directory, which pulls `examples/` and analyzer `testdata/` into repo-root dogfooding | decide whether later CLI/boundary work should expose explicit owned-root or ignore-root controls rather than relying on caller working-directory discipline | `Phase C` |
| `B2-CLI-003` | repo-wide `check` / `clippy` / `ci` | the current CLI dogfood line is predictably red because B1 findings are intentionally still present in source and test projects | keep the initial enforcement posture non-blocking until the dogfood findings are remediated or triaged sprint-by-sprint | `B2` |
| `B2-CLI-004` | concurrent local `dotnet run` commands | parallel wrapper invocation in one worktree can race on intermediate build outputs and fail with `CS2012` file locks | if concurrent local dogfooding becomes an expected workflow, add repo guidance or a future wrapper strategy; do not treat this as a contract defect in Phase B | `later phase` |

## Phase C Escalation Candidates

- `ToolId` and `ToolDescriptor`
  - B2 showed these ids/descriptors are the stable inventory surface the later
    boundary package should reuse when it models owned tools and command
    families
- `CliEnvelope<T>`, `CliError`, `CliDiagnostic`, `CliErrorKind`
  - B2 confirmed the top-level machine envelope is already a stable cross-tool
    pattern that should remain aligned with later boundary-package outputs
- `ILintToolModule`, `ILintToolCommandHandler<TRequest, TResponse>`,
  `LintToolRequest`, and `LintToolResult`
  - B2 exposed repo-root targeting and backend-normalization concerns that
    should inform the later boundary package command/discovery seams
- `ILintWorkspaceAdapter`
  - B2 exposed the importance of making repo-root scope explicit, which is the
    same class of ownership and root-selection problem the boundary package will
    need to solve more strictly
