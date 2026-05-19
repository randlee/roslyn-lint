# sc.lint.roslyn.demagic analyzer sample corpus

This directory is the production-testing sample corpus for `sc.lint.roslyn.demagic`.

Layout:

- `DM001/`
  constant-consolidation samples for positive, negative, suppression,
  config-failure, severity, and designated-class coverage
- `DM002/`
  forbidden-string samples for positive, negative, suppression,
  config-failure, severity, and expression-context coverage

Conventions:

- each file compiles as a standalone analyzer input sample
- file names describe the intended coverage scenario
- analyzer tests load these fixtures directly instead of embedding ad hoc code
- `TestMatrix.md` maps every requirement and PRD checklist item to one or more
  sample files and owning test methods
