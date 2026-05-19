# sc.lint.roslyn.demagic Permutation Matrix

This document is the authoritative closed inventory of supported
`sc.lint.roslyn.demagic` v1 analyzer permutations.

This matrix records behaviorally distinct analyzer branches, not a redundant
cartesian expansion of cells that execute the same matcher or suppression path.
Every supported row below is backed by a concrete automated test. Any behavior
that is not supported by v1 is listed explicitly as `unsupported`.

`sc.lint.roslyn.demagic` is not publish-ready unless every row in this document is
either `covered` or `unsupported`.

## Evidence Convention

- analyzer-corpus fixtures live under `tests/sc.lint.roslyn.demagic.tests/testdata/`
- package-smoke fixtures live under
  `examples/sc.lint.roslyn.demagic.package-smoke/samples/`
- test methods live under `tests/sc.lint.roslyn.demagic.tests/analyzers/`

## DM001 Covered Permutations

| ID | Supported behavior | Evidence | Package-smoke evidence | State |
| --- | --- | --- | --- | --- |
| `DM001-001` | `public const` outside the designated file reports a diagnostic | `DM001/PublicConstOutsideDesignatedFile.cs`; `PositiveSamples_ReportExpectedDiagnostics` | `samples/dm001/PublicConstOutsideDesignatedFile.cs` manifest entry | `covered` |
| `DM001-002` | `internal const` outside the designated file reports a diagnostic | `DM001/InternalConstOutsideDesignatedFile.cs`; `PositiveSamples_ReportExpectedDiagnostics` | `samples/dm001/InternalConstOutsideDesignatedFile.cs` manifest entry | `covered` |
| `DM001-003` | configured `designated_class` mismatch reports a diagnostic | `DM001/DesignatedClassMismatch.cs`; `PositiveSamples_ReportExpectedDiagnostics` | not part of the package-smoke subset | `covered` |
| `DM001-004` | matching designated file suppresses the diagnostic, including case-insensitive file-name comparison | `DM001/DesignatedFileCompliantConst.cs`; `NegativeAndConfigSamples_DoNotReport` with `CompliantConfig` | `samples/dm001/DesignatedFileCompliantConst.cs` expected-clean entry | `covered` |
| `DM001-005` | matching designated class suppresses the diagnostic, including case-insensitive class-name comparison | `DM001/DesignatedFileCompliantConst.cs`; `NegativeAndConfigSamples_DoNotReport` with `CaseInsensitiveClassConfig` | not part of the package-smoke subset | `covered` |
| `DM001-006` | non-target visibilities (`private`, `protected`, `private protected`, `protected internal`) are ignored | `DM001/PrivateProtectedIgnored.cs`; `NegativeAndConfigSamples_DoNotReport` | not part of the package-smoke subset | `covered` |
| `DM001-007` | local `const` declarations are ignored | `DM001/LocalConstIgnored.cs`; `NegativeAndConfigSamples_DoNotReport` | not part of the package-smoke subset | `covered` |
| `DM001-008` | missing config fails closed with no diagnostic | `DM001/MissingConfigNoDiagnostics.cs`; `NegativeAndConfigSamples_DoNotReport` | not part of the package-smoke subset | `covered` |
| `DM001-009` | disabled config produces no diagnostic | `DM001/PublicConstOutsideDesignatedFile.cs`; `NegativeAndConfigSamples_DoNotReport` with `DisabledConfig` | not part of the package-smoke subset | `covered` |
| `DM001-010` | invalid severity fails closed with no diagnostic | `DM001/PublicConstOutsideDesignatedFile.cs`; `NegativeAndConfigSamples_DoNotReport` with `InvalidSeverityConfig` | not part of the package-smoke subset | `covered` |
| `DM001-011` | configured severity `hidden` is honored | `DM001/PublicConstOutsideDesignatedFile.cs`; `SeverityFromConfig_UsesConfiguredSeverity` | not part of the package-smoke subset | `covered` |
| `DM001-012` | configured severity `info` is honored | `DM001/PublicConstOutsideDesignatedFile.cs`; `SeverityFromConfig_UsesConfiguredSeverity` | not part of the package-smoke subset | `covered` |
| `DM001-013` | configured severity `warning` is honored | `DM001/PublicConstOutsideDesignatedFile.cs`; `PositiveSamples_ReportExpectedDiagnostics` | `samples/dm001/PublicConstOutsideDesignatedFile.cs` manifest entry | `covered` |
| `DM001-014` | configured severity `error` is honored | `DM001/PublicConstOutsideDesignatedFile.cs`; `SeverityFromConfig_UsesConfiguredSeverity` | not part of the package-smoke subset | `covered` |
| `DM001-015` | `#pragma warning disable DM001` suppresses the diagnostic | `DM001/SuppressedConst.cs`; `PragmaSuppression_Works` | `samples/dm001/SuppressedConst.cs` expected-clean entry | `covered` |
| `DM001-016` | the unsuppressed control for the suppression sample still reports a diagnostic | `DM001/SuppressedConst.cs`; `PragmaSuppression_Works` unsuppressed control branch | `samples/dm001/UnsuppressedConstControl.cs` manifest entry | `covered` |

## DM002 Covered Permutations

| ID | Supported behavior | Evidence | Package-smoke evidence | State |
| --- | --- | --- | --- | --- |
| `DM002-001` | exact-pattern match in a const field reports a diagnostic | `DM002/ExactMatchConstField.cs`; `PositiveSamples_ReportExpectedDiagnostics` | `samples/dm002/ExactMatchConstField.cs` manifest entry | `covered` |
| `DM002-002` | prefix-pattern match in a method argument reports a diagnostic | `DM002/PrefixMethodArgument.cs`; `PositiveSamples_ReportExpectedDiagnostics` | `samples/dm002/PrefixMethodArgument.cs` manifest entry | `covered` |
| `DM002-003` | suffix-pattern match in a comparison reports a diagnostic | `DM002/SuffixComparison.cs`; `PositiveSamples_ReportExpectedDiagnostics` | `samples/dm002/SuffixComparison.cs` manifest entry | `covered` |
| `DM002-004` | substring-pattern match in a return expression reports a diagnostic | `DM002/SubstringReturnValue.cs`; `PositiveSamples_ReportExpectedDiagnostics` | `samples/dm002/SubstringReturnValue.cs` manifest entry | `covered` |
| `DM002-005` | attribute-argument literals are analyzed | `DM002/AttributeArgument.cs`; `PositiveSamples_ReportExpectedDiagnostics` | `samples/dm002/AttributeArgument.cs` manifest entry | `covered` |
| `DM002-006` | switch-arm literals are analyzed | `DM002/SwitchArmLiteral.cs`; `PositiveSamples_ReportExpectedDiagnostics` | not part of the package-smoke subset | `covered` |
| `DM002-007` | default matching is case-insensitive | `DM002/CaseSensitiveMismatch.cs`; `PositiveSamples_ReportExpectedDiagnostics` with `case_sensitive = false` | not part of the package-smoke subset | `covered` |
| `DM002-008` | `case_sensitive = true` reports a diagnostic when casing is identical | `DM002/ExactMatchConstField.cs`; `PositiveSamples_ReportExpectedDiagnostics` with `case_sensitive = true` | not part of the package-smoke subset | `covered` |
| `DM002-009` | `case_sensitive = true` suppresses diagnostics when casing differs | `DM002/CaseSensitiveMismatch.cs`; `NegativeAndConfigSamples_DoNotReport` with `case_sensitive = true` | not part of the package-smoke subset | `covered` |
| `DM002-010` | non-matching literals do not report diagnostics | `DM002/NonMatchingLiteral.cs`; `NegativeAndConfigSamples_DoNotReport` | `samples/dm002/CompliantLiteral.cs` expected-clean entry | `covered` |
| `DM002-011` | interpolated-string holes are ignored | `DM002/InterpolatedHole.cs`; `NegativeAndConfigSamples_DoNotReport` | unsupported in package smoke because interpolated-hole diagnostics are intentionally excluded from the v1 smoke subset | `covered` |
| `DM002-012` | comments are ignored | `DM002/CommentsAndDocumentationIgnored.cs`; `NegativeAndConfigSamples_DoNotReport` | unsupported in package smoke because comments are not analyzer diagnostics | `covered` |
| `DM002-013` | XML documentation is ignored | `DM002/CommentsAndDocumentationIgnored.cs`; `NegativeAndConfigSamples_DoNotReport` | unsupported in package smoke because XML docs are not analyzer diagnostics | `covered` |
| `DM002-014` | missing config fails closed with no diagnostic | `DM002/MissingConfigNoDiagnostics.cs`; `NegativeAndConfigSamples_DoNotReport` | unsupported in package smoke because the smoke project is intentionally configured | `covered` |
| `DM002-015` | disabled config produces no diagnostic | `DM002/ExactMatchConstField.cs`; `NegativeAndConfigSamples_DoNotReport` with disabled config | unsupported in package smoke because the smoke project is intentionally configured | `covered` |
| `DM002-016` | invalid severity fails closed with no diagnostic | `DM002/InvalidConfigNoDiagnostics.cs`; `NegativeAndConfigSamples_DoNotReport` | unsupported in package smoke because invalid-config scenarios stay in the analyzer corpus | `covered` |
| `DM002-017` | configured severity `hidden` is honored | `DM002/SeverityFromConfig.cs`; `SeverityFromConfig_UsesConfiguredSeverity` | unsupported in package smoke because the smoke manifest asserts the shipping warning profile only | `covered` |
| `DM002-018` | configured severity `info` is honored | `DM002/AttributeArgument.cs`; `PositiveSamples_ReportExpectedDiagnostics` and `DM002/SeverityFromConfig.cs`; `SeverityFromConfig_UsesConfiguredSeverity` | unsupported in package smoke because the smoke manifest asserts the shipping warning profile only | `covered` |
| `DM002-019` | configured severity `warning` is honored | `DM002/PrefixMethodArgument.cs`; `PositiveSamples_ReportExpectedDiagnostics` and `DM002/SeverityFromConfig.cs`; `SeverityFromConfig_UsesConfiguredSeverity` | `samples/dm002/PrefixMethodArgument.cs` manifest entry | `covered` |
| `DM002-020` | configured severity `error` is honored | `DM002/ExactMatchConstField.cs`; `PositiveSamples_ReportExpectedDiagnostics` and `DM002/SeverityFromConfig.cs`; `SeverityFromConfig_UsesConfiguredSeverity` | unsupported in package smoke because the smoke manifest asserts the shipping warning profile only | `covered` |
| `DM002-021` | `#pragma warning disable DM002` suppresses the diagnostic | `DM002/SuppressedLiteral.cs`; `PragmaSuppression_Works` | `samples/dm002/SuppressedLiteral.cs` expected-clean entry | `covered` |
| `DM002-022` | the unsuppressed control for the suppression sample still reports a diagnostic | `DM002/SuppressedLiteral.cs`; `PragmaSuppression_Works` unsuppressed control branch | `samples/dm002/UnsuppressedLiteralControl.cs` manifest entry | `covered` |

## Unsupported By Design In v1

| ID | Permutation | Reason | State |
| --- | --- | --- | --- |
| `UNSUPPORTED-001` | `DM001` on `static readonly` fields | deferred by the PRD; v1 only governs `const` fields | `unsupported` |
| `UNSUPPORTED-002` | diagnostics inside interpolated-string holes | explicitly excluded from the requirements | `unsupported` |
| `UNSUPPORTED-003` | diagnostics on comments or XML documentation text | `DM002` is syntax-literal scoped only | `unsupported` |
| `UNSUPPORTED-004` | code-fix permutations for either rule | code fixes are out of scope for v1 | `unsupported` |
