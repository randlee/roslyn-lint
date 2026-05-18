# ADR-006 — Packaged Analyzer Validation And Staged Publication Policy

| Field | Value |
|---|---|
| ID | ADR-006 |
| Status | **Accepted** |
| Date | 2026-05-18 |
| Deciders | Rand Lee |
| Relates to | REQ-DM-PACK-006, REQ-DM-PACK-007, REQ-DM-TEST-007, REQ-DM-TEST-009, REQ-DM-TEST-010 |
| Supersedes | — |

---

## Context

Phase A exits only when `Roslyn.DeMagic` is ready for production testing as a
packaged analyzer, not merely as an in-repo project reference. The planning
line now includes:

- a local-feed package-consumer example
- expected-diagnostic manifest checking
- CI validation of the packaged-consumer path
- GitHub Packages publication from CI
- a documented but still-manual first NuGet.org release

Without an explicit ADR, these release and validation rules could drift across
phase docs, workflows, and package documentation.

## Decision Drivers

- package-consumer validation must prove the real deliverable shape
- CI must enforce the same packaged path that local validation uses
- GitHub Packages publication is useful immediately, but NuGet.org automation
  should wait until the first package is manually proven
- release docs, manifests, and workflows must describe one consistent path

## Decision

`Roslyn.DeMagic` production readiness is gated by packaged-consumer validation,
and publication is staged.

This means:

- local-feed package-consumer validation is a required release gate
- expected diagnostics are asserted from a structured manifest rather than
  manual console review
- CI must run the packaged-consumer validation path before publication is
  considered passing
- CI may publish repo-produced packages to GitHub Packages once validation
  passes
- the first NuGet.org publication remains manual and must be documented in
  `docs/releasing.md`

## Consequences

### Positive

- the analyzer is validated in the same form users consume
- CI, local validation scripts, and release docs stay aligned
- NuGet.org automation is deferred until a real first-release path is proven

### Negative

- Phase A gains additional validation artifacts and workflow complexity
- release readiness now depends on maintaining scripts, manifests, and sample
  projects together

## Follow-Up Work

- keep `docs/phase-A/sprint-A11.md`, `sprint-A12.md`, and `sprint-A13.md`
  aligned with this policy
- add and maintain the expected-diagnostics manifest
- document the manual first-release NuGet.org handoff in `docs/releasing.md`
