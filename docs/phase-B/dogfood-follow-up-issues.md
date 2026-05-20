# B1 Analyzer Dogfood Follow-Up Issues

## Row Contract

| Id | Finding source | Problem | Required follow-up | Owner phase/sprint |
| --- | --- | --- | --- | --- |
| `B1-DM001-001` | `src/sc.lint.roslyn.demagic` warm self-analysis | The analyzer project only reports its own DM001 findings after a warm second build because self-analysis depends on the built analyzer output already existing. | Make self-hosted analyzer dogfooding deterministic on a cold build, or document and automate a stable warm-build step for analyzer self-analysis. | `Phase B / B1 follow-up` |
| `B1-DM002-001` | `src/sc.lint.roslyn.demagic.lint`; `src/sc.lint.roslyn`; `tests/sc.lint.roslyn.tests` | DM002 currently treats shipped tool/CLI identifiers like `demagic` and `sc-lint-roslyn` as forbidden product-coupling strings. The behavior is predictable, but the current config is too blunt for command-surface vocabulary. | Decide whether command-surface identifiers should be centralized as shared constants, config-exempted, or explicitly allowed as suite vocabulary. Update config and code policy accordingly. | `Phase B / B2` |
| `B1-DM002-002` | `tests/sc.lint.roslyn.tests` | Repeated test assertions on public command/tool ids create high-volume DM002 noise once the analyzer is wired into local test builds. | Define whether test assemblies should centralize public-surface literals behind shared test constants or remain literal with documented suppression/config policy. | `Phase B / B2` |

## Rule

- every analyzer predictability gap, false positive, false negative, or unclear
  rule behavior that is not closed inside `B1` must have an entry here
