# Roslyn.DeMagic Test Matrix

This matrix ties the approved `Roslyn.DeMagic` analyzer-behavior,
configuration, and validation requirements exercised in A10 to concrete
automated samples and tests.

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
| `REQ-DM-CONFIG-002` | config | `.roslyn-lint/config-src.toml` | `TryLoad_ConfigSrcToml_ParsesExpectedSettings` | config |
| `REQ-DM-CONFIG-003` | config | `.roslyn-lint/config-test.toml` | `TryLoad_CaseSensitiveTrue_ParsesExpectedSettings` | config |
| `REQ-DM-CONFIG-005` | `DM001`, `DM002` | `DM001/MissingConfigNoDiagnostics.cs`, `DM002/MissingConfigNoDiagnostics.cs` | `NegativeAndConfigSamples_DoNotReport` | config-failure |
| `REQ-DM-CONFIG-008` | `DM001`, `DM002` | `DM001/PublicConstOutsideDesignatedFile.cs`, `DM002/InvalidConfigNoDiagnostics.cs` | `NegativeAndConfigSamples_DoNotReport` | config-failure |
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
| Private `const` field does not raise `DM001` | `DM001/PrivateProtectedIgnored.cs` | `NegativeAndConfigSamples_DoNotReport` |
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
| `config-src.toml` is loaded for non-test projects | `.roslyn-lint/config-src.toml` | `TryLoad_ConfigSrcToml_ParsesExpectedSettings` |
| `config-test.toml` is loaded for test projects | `.roslyn-lint/config-test.toml` | `TryLoad_CaseSensitiveTrue_ParsesExpectedSettings` |
| Missing config file disables all rules gracefully | `DM001/MissingConfigNoDiagnostics.cs`, `DM002/MissingConfigNoDiagnostics.cs` | `NegativeAndConfigSamples_DoNotReport` |
| Malformed config disables the affected rule without crashing analysis | `DM001/PublicConstOutsideDesignatedFile.cs`, `DM002/InvalidConfigNoDiagnostics.cs` | `NegativeAndConfigSamples_DoNotReport` |

## A11 Foundation

The corpus is intentionally file-based so A11 can reuse these same samples when
the analyzer is validated from a locally packed NuGet package.
