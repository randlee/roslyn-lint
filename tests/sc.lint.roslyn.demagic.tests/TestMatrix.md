# sc.lint.roslyn.demagic Test Matrix

This matrix ties the approved `sc.lint.roslyn.demagic` analyzer-behavior,
configuration, and validation requirements exercised in A10 to concrete
automated samples and tests.

## A12 Production-Readiness Signoff

Phase A production-readiness requires this matrix to close with no requirement
gaps for the shipped analyzer set.

- shipped rule set: `DM001`, `DM002`
- requirement rows: every approved `REQ-DM*` row below maps to concrete
  analyzer samples and automated test methods
- permutation closure: `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md`
  remains the authoritative supported-permutation inventory for publish
  signoff
- packaged-consumer closure: `examples/sc.lint.roslyn.demagic.package-smoke/` plus
  `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json` are the authoritative
  package-consumer validation artifacts for Phase A v0.2.0

## Requirement Coverage

| Requirement | Rule | Sample(s) | Owning test method(s) | Kind |
| --- | --- | --- | --- | --- |
| `REQ-DM001-001` | `DM001` | `DM001/PublicConstOutsideDesignatedFile.cs`, `DM001/InternalConstOutsideDesignatedFile.cs` | `PositiveSamples_ReportExpectedDiagnostics` | positive |
| `REQ-DM001-002` | `DM001` | `DM001/PublicConstOutsideDesignatedFile.cs`, `DM001/DesignatedFileCompliantConst.cs` | `PositiveSamples_ReportExpectedDiagnostics`, `NegativeAndConfigSamples_DoNotReport` | positive, negative |
| `REQ-DM001-003` | `DM001` | `DM001/DesignatedClassMismatch.cs`, `DM001/DesignatedFileCompliantConst.cs` | `PositiveSamples_ReportExpectedDiagnostics`, `NegativeAndConfigSamples_DoNotReport` | positive, negative |
| `REQ-DM001-004` | `DM001` | `DM001/PrivateProtectedIgnored.cs` | `NegativeAndConfigSamples_DoNotReport` | negative |
| `REQ-DM001-005` | `DM001` | `DM001/LocalConstIgnored.cs` | `NegativeAndConfigSamples_DoNotReport` | negative |
| `REQ-DM001-006` | `DM001` | `DM001/SuppressedConst.cs` | `PragmaSuppression_Works` | suppression |
| `REQ-DM002-001` | `DM002` | `DM002/ExactMatchConstField.cs`, `DM002/PrefixMethodArgument.cs`, `DM002/SuffixComparison.cs`, `DM002/SubstringReturnValue.cs`, `DM002/AttributeArgument.cs`, `DM002/SwitchArmLiteral.cs` | `PositiveSamples_ReportExpectedDiagnostics` | positive |
| `REQ-DM002-002` | `DM002` | `DM002/ExactMatchConstField.cs`, `DM002/PrefixMethodArgument.cs`, `DM002/SuffixComparison.cs`, `DM002/SubstringReturnValue.cs` | `PositiveSamples_ReportExpectedDiagnostics` | positive |
| `REQ-DM002-003` | `DM002` | `DM002/CaseSensitiveMismatch.cs` | `PositiveSamples_ReportExpectedDiagnostics`, `NegativeAndConfigSamples_DoNotReport` | corner-case |
| `REQ-DM002-004` | `DM002` | `DM002/ExactMatchConstField.cs`, `DM002/PrefixMethodArgument.cs`, `DM002/SuffixComparison.cs`, `DM002/SubstringReturnValue.cs`, `DM002/AttributeArgument.cs`, `DM002/SwitchArmLiteral.cs` | `PositiveSamples_ReportExpectedDiagnostics` | positive |
| `REQ-DM002-005` | `DM002` | `DM002/InterpolatedHole.cs`, `DM002/CommentsAndDocumentationIgnored.cs` | `NegativeAndConfigSamples_DoNotReport` | corner-case |
| `REQ-DM002-006` | `DM002` | `DM002/SuppressedLiteral.cs` | `PragmaSuppression_Works` | suppression |
| `REQ-DM-CONFIG-002` | config | `.sc-lint-roslyn/config-src.toml` | `TryLoad_ConfigSrcToml_ParsesExpectedSettings` | config |
| `REQ-DM-CONFIG-003` | config | `.sc-lint-roslyn/config-test.toml` | `TryLoad_ConfigTestToml_WinsAndDoesNotMergeSourceConfig`, `TryLoad_CaseSensitiveTrue_ParsesExpectedSettings` | config |
| `REQ-DM-CONFIG-005` | `DM001`, `DM002` | `DM001/MissingConfigNoDiagnostics.cs`, `DM002/MissingConfigNoDiagnostics.cs` | `NegativeAndConfigSamples_DoNotReport`, `TryLoad_NoConfigFiles_ReturnsDisabledConfig` | config-failure |
| `REQ-DM-CONFIG-008` | `DM001`, `DM002` | `DM001/PublicConstOutsideDesignatedFile.cs`, `DM002/InvalidConfigNoDiagnostics.cs`, `.sc-lint-roslyn/config-test.toml` | `NegativeAndConfigSamples_DoNotReport`, `TryLoad_InvalidSeverity_ReturnsErrors` | config-failure |
| `REQ-DM-DIAG-001` | `DM001`, `DM002` | `DM001/PublicConstOutsideDesignatedFile.cs`, `DM002/ExactMatchConstField.cs` | `PositiveSamples_ReportExpectedDiagnostics` | positive |
| `REQ-DM-DIAG-002` | `DM001`, `DM002` | `DM001/PublicConstOutsideDesignatedFile.cs`, `DM002/ExactMatchConstField.cs` | `PositiveSamples_ReportExpectedDiagnostics` | positive |
| `REQ-DM-DIAG-003` | `DM001`, `DM002` | `DM001/SeverityFromConfig.cs`, `DM002/SeverityFromConfig.cs` | `SeverityFromConfig_UsesConfiguredSeverity` | severity |
| `REQ-DM-DIAG-005` | `DM001`, `DM002` | `DM001/PublicConstOutsideDesignatedFile.cs`, `DM002/ExactMatchConstField.cs` | `PositiveSamples_ReportExpectedDiagnostics` | positive |
| `REQ-DM-TEST-001` | `DM001`, `DM002` | full corpus | all analyzer test methods | mixed |
| `REQ-DM-TEST-002` | `DM001`, `DM002` | `DM001/SeverityFromConfig.cs`, `DM002/SeverityFromConfig.cs` | `SeverityFromConfig_UsesConfiguredSeverity` | severity |
| `REQ-DM-TEST-003` | `DM001`, `DM002` | `DM001/MissingConfigNoDiagnostics.cs`, `DM002/MissingConfigNoDiagnostics.cs` | `NegativeAndConfigSamples_DoNotReport` | config-failure |
| `REQ-DM-TEST-006` | `DM001`, `DM002` | full corpus | all analyzer test methods | mixed |
| `REQ-DM-TEST-008` | `DM001`, `DM002` | this matrix plus full corpus | `CoverageMatrix_CoversApprovedRequirements` | traceability |

## PRD Checklist Coverage

### DM001

| PRD checklist item | Sample(s) | Owning test method |
| --- | --- | --- |
| Public `const` field outside designated file raises `DM001` | `DM001/PublicConstOutsideDesignatedFile.cs` | `PositiveSamples_ReportExpectedDiagnostics` |
| Internal `const` field outside designated file raises `DM001` | `DM001/InternalConstOutsideDesignatedFile.cs` | `PositiveSamples_ReportExpectedDiagnostics` |
| Non-public-or-internal `const` visibilities stay out of scope in v1, including `private`, `protected`, `private protected`, and `protected internal` | `DM001/PrivateProtectedIgnored.cs` | `NegativeAndConfigSamples_DoNotReport` |
| Local `const` inside a method body does not raise `DM001` | `DM001/LocalConstIgnored.cs` | `NegativeAndConfigSamples_DoNotReport` |
| `const` in the designated file does not raise `DM001` | `DM001/DesignatedFileCompliantConst.cs` | `NegativeAndConfigSamples_DoNotReport` |
| Rule is suppressible via `#pragma warning disable DM001` | `DM001/SuppressedConst.cs` | `PragmaSuppression_Works` |
| Severity is respected from config | `DM001/SeverityFromConfig.cs` | `SeverityFromConfig_UsesConfiguredSeverity` |

### DM002

| PRD checklist item | Sample(s) | Owning test method |
| --- | --- | --- |
| Exact match raises `DM002` | `DM002/ExactMatchConstField.cs` | `PositiveSamples_ReportExpectedDiagnostics` |
| Prefix wildcard raises `DM002` | `DM002/PrefixMethodArgument.cs` | `PositiveSamples_ReportExpectedDiagnostics` |
| Suffix wildcard raises `DM002` | `DM002/SuffixComparison.cs` | `PositiveSamples_ReportExpectedDiagnostics` |
| Substring wildcard raises `DM002` | `DM002/SubstringReturnValue.cs` | `PositiveSamples_ReportExpectedDiagnostics` |
| Non-matching literals do not raise `DM002` | `DM002/NonMatchingLiteral.cs` | `NegativeAndConfigSamples_DoNotReport` |
| Case-insensitive matching is the default | `DM002/CaseSensitiveMismatch.cs` | `PositiveSamples_ReportExpectedDiagnostics` |
| `case_sensitive = true` config is respected | `DM002/CaseSensitiveMismatch.cs` | `NegativeAndConfigSamples_DoNotReport` |
| Violation in const declaration raises `DM002` | `DM002/ExactMatchConstField.cs` | `PositiveSamples_ReportExpectedDiagnostics` |
| Violation in comparison expression raises `DM002` | `DM002/SuffixComparison.cs` | `PositiveSamples_ReportExpectedDiagnostics` |
| Violation in method argument raises `DM002` | `DM002/PrefixMethodArgument.cs` | `PositiveSamples_ReportExpectedDiagnostics` |
| Rule is suppressible via `#pragma warning disable DM002` | `DM002/SuppressedLiteral.cs` | `PragmaSuppression_Works` |
| Severity is respected from config | `DM002/SeverityFromConfig.cs` | `SeverityFromConfig_UsesConfiguredSeverity` |

### Config

| PRD checklist item | Sample(s) | Owning test method |
| --- | --- | --- |
| `config-src.toml` is loaded for non-test projects | `.sc-lint-roslyn/config-src.toml` | `TryLoad_ConfigSrcToml_ParsesExpectedSettings` |
| `config-test.toml` is loaded for test projects and remains fully independent from `config-src.toml` with no merging or inheritance | `.sc-lint-roslyn/config-test.toml`, `.sc-lint-roslyn/config-src.toml` | `TryLoad_ConfigTestToml_WinsAndDoesNotMergeSourceConfig` |
| Missing config file disables all rules gracefully | `DM001/MissingConfigNoDiagnostics.cs`, `DM002/MissingConfigNoDiagnostics.cs` | `NegativeAndConfigSamples_DoNotReport`, `TryLoad_NoConfigFiles_ReturnsDisabledConfig` |
| Malformed config disables the affected rule without crashing analysis | `DM001/PublicConstOutsideDesignatedFile.cs`, `DM002/InvalidConfigNoDiagnostics.cs` | `NegativeAndConfigSamples_DoNotReport` |
| Malformed config emits loader errors instead of failing silently | `.sc-lint-roslyn/config-test.toml` | `TryLoad_InvalidSeverity_ReturnsErrors` |

## A11 Foundation

The corpus is intentionally file-based so A11 can reuse these same samples when
the analyzer is validated from a locally packed NuGet package.
