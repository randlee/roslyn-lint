# Roslyn.DeMagic Permutation Matrix

This document is the authoritative closed inventory of supported
`Roslyn.DeMagic` v1 analyzer permutations.

No supported permutation is implicitly covered by prose.
Every supported permutation must appear in this document with one of these
states:

- `covered`: a concrete fixture and automated test already exist
- `planned`: the permutation is supported and must receive a concrete fixture
  and automated test before publish signoff
- `unsupported`: the permutation is explicitly out of scope for v1

`Roslyn.DeMagic` is not publish-ready until every row in this document is
either `covered` or `unsupported`.

## Fixture Convention

- analyzer-corpus fixtures live under `tests/Roslyn.DeMagic.Tests/TestData/`
- package-smoke fixtures live under
  `examples/Roslyn.DeMagic.PackageSmoke/Samples/`
- canonical sample ids use:
  `Rule.Dimension.Dimension.[CaseMode.]Outcome`

Examples:

- `DM001.Public.FileMismatch.Warning`
- `DM002.Exact.Const.DefaultCase.Match`
- `DM002.Substring.Attribute.CaseSensitive.NonMatch`

## DM001 Supported Dimensions

- visibilities:
  `public`, `internal`, `private`, `protected`, `private protected`,
  `protected internal`, `local const`
- designated file modes:
  `match`, `mismatch`, `case-insensitive match`
- designated class modes:
  `not configured`, `configured and match`,
  `configured and mismatch`, `configured and case-insensitive match`
- config states:
  `enabled`, `disabled`, `missing`, `invalid severity`
- severity states:
  `hidden`, `info`, `warning`, `error`
- suppression states:
  `suppressed`, `unsuppressed`

## DM001 Permutations

### Diagnostic Behavior

| ID | Visibility | Designated file | Designated class | Expected | Analyzer corpus fixture | Package smoke fixture | State |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `DM001-001` | `public` | mismatch | not configured | diagnostic | `DM001/PublicConstOutsideDesignatedFile.cs` | `Samples/DM001/PublicConstOutsideDesignatedFile.cs` | `covered` |
| `DM001-002` | `internal` | mismatch | not configured | diagnostic | `DM001/InternalConstOutsideDesignatedFile.cs` | `Samples/DM001/InternalConstOutsideDesignatedFile.cs` | `covered` |
| `DM001-003` | `public` | mismatch | configured and mismatch | diagnostic | `DM001/DesignatedClassMismatch.cs` | `Samples/DM001/PublicConstDesignatedClassMismatch.cs` | `planned` |
| `DM001-004` | `internal` | mismatch | configured and mismatch | diagnostic | `DM001/InternalConstDesignatedClassMismatch.cs` | `Samples/DM001/InternalConstDesignatedClassMismatch.cs` | `planned` |
| `DM001-005` | `public` | match | configured and match | no diagnostic | `DM001/DesignatedFileCompliantConst.cs` | `Samples/DM001/DesignatedFileCompliantConst.cs` | `covered` |
| `DM001-006` | `internal` | match | configured and match | no diagnostic | `DM001/InternalDesignatedFileCompliantConst.cs` | `Samples/DM001/InternalDesignatedFileCompliantConst.cs` | `planned` |
| `DM001-007` | `public` | case-insensitive match | configured and match | no diagnostic | `DM001/CaseInsensitiveDesignatedFileMatch.cs` | `Samples/DM001/CaseInsensitiveDesignatedFileMatch.cs` | `planned` |
| `DM001-008` | `public` | match | configured and case-insensitive match | no diagnostic | `DM001/CaseInsensitiveDesignatedClassMatch.cs` | `Samples/DM001/CaseInsensitiveDesignatedClassMatch.cs` | `planned` |

### Ignored Visibility And Scope Behavior

| ID | Shape | Expected | Analyzer corpus fixture | Package smoke fixture | State |
| --- | --- | --- | --- | --- | --- |
| `DM001-009` | `private const` field outside designated file | no diagnostic | `DM001/PrivateProtectedIgnored.cs` | `Samples/DM001/PrivateIgnoredConst.cs` | `planned` |
| `DM001-010` | `protected const` field outside designated file | no diagnostic | `DM001/PrivateProtectedIgnored.cs` | `Samples/DM001/ProtectedIgnoredConst.cs` | `planned` |
| `DM001-011` | `private protected const` field outside designated file | no diagnostic | `DM001/PrivateProtectedIgnored.cs` | `Samples/DM001/PrivateProtectedIgnoredConst.cs` | `planned` |
| `DM001-012` | `protected internal const` field outside designated file | no diagnostic | `DM001/PrivateProtectedIgnored.cs` | `Samples/DM001/ProtectedInternalIgnoredConst.cs` | `planned` |
| `DM001-013` | local `const` inside a method body | no diagnostic | `DM001/LocalConstIgnored.cs` | `Samples/DM001/LocalConstIgnored.cs` | `planned` |

### Config, Severity, And Suppression

| ID | Mode | Expected | Analyzer corpus fixture | Package smoke fixture | State |
| --- | --- | --- | --- | --- | --- |
| `DM001-014` | missing config | no diagnostic | `DM001/MissingConfigNoDiagnostics.cs` | `Samples/DM001/MissingConfigNoDiagnostics.cs` | `planned` |
| `DM001-015` | disabled config | no diagnostic | `DM001/PublicConstOutsideDesignatedFile.cs` | `Samples/DM001/DisabledConfigNoDiagnostics.cs` | `planned` |
| `DM001-016` | invalid severity | no diagnostic | `DM001/PublicConstOutsideDesignatedFile.cs` | `Samples/DM001/InvalidSeverityNoDiagnostics.cs` | `planned` |
| `DM001-017` | severity `hidden` | hidden diagnostic | `DM001/SeverityHidden.cs` | `Samples/DM001/SeverityHidden.cs` | `planned` |
| `DM001-018` | severity `info` | info diagnostic | `DM001/SeverityInfo.cs` | `Samples/DM001/SeverityInfo.cs` | `planned` |
| `DM001-019` | severity `warning` | warning diagnostic | `DM001/PublicConstOutsideDesignatedFile.cs` | `Samples/DM001/PublicConstOutsideDesignatedFile.cs` | `covered` |
| `DM001-020` | severity `error` | error diagnostic | `DM001/SeverityFromConfig.cs` | `Samples/DM001/SeverityFromConfig.cs` | `planned` |
| `DM001-021` | `#pragma` suppression | no diagnostic | `DM001/SuppressedConst.cs` | `Samples/DM001/SuppressedConst.cs` | `covered` |
| `DM001-022` | unsuppressed control for suppression sample | diagnostic | `DM001/SuppressedConst.cs` | `Samples/DM001/UnsuppressedConstControl.cs` | `covered` |

## DM002 Supported Dimensions

- pattern kinds:
  `exact`, `prefix`, `suffix`, `substring`
- syntax contexts:
  `const field`, `comparison`, `method argument`, `return value`,
  `attribute argument`, `switch arm`
- case modes:
  default case-insensitive, `case_sensitive = true`
- match modes:
  matching literal, non-matching literal
- ignored content:
  comments, XML documentation, interpolated string holes
- config states:
  `enabled`, `disabled`, `missing`, `invalid severity`
- severity states:
  `hidden`, `info`, `warning`, `error`
- suppression states:
  `suppressed`, `unsuppressed`

## DM002 Permutations

### Default Case-Insensitive Matching: Positive Matrix

Expected result for every cell in this matrix: `DM002` diagnostic reported.

| Pattern kind \ Context | Const field | Comparison | Method argument | Return value | Attribute argument | Switch arm |
| --- | --- | --- | --- | --- | --- | --- |
| exact | `DM002.Exact.Const.DefaultCase.Match` `covered` | `DM002.Exact.Comparison.DefaultCase.Match` `planned` | `DM002.Exact.MethodArgument.DefaultCase.Match` `planned` | `DM002.Exact.Return.DefaultCase.Match` `planned` | `DM002.Exact.Attribute.DefaultCase.Match` `planned` | `DM002.Exact.SwitchArm.DefaultCase.Match` `planned` |
| prefix | `DM002.Prefix.Const.DefaultCase.Match` `planned` | `DM002.Prefix.Comparison.DefaultCase.Match` `planned` | `DM002.Prefix.MethodArgument.DefaultCase.Match` `covered` | `DM002.Prefix.Return.DefaultCase.Match` `planned` | `DM002.Prefix.Attribute.DefaultCase.Match` `planned` | `DM002.Prefix.SwitchArm.DefaultCase.Match` `planned` |
| suffix | `DM002.Suffix.Const.DefaultCase.Match` `planned` | `DM002.Suffix.Comparison.DefaultCase.Match` `covered` | `DM002.Suffix.MethodArgument.DefaultCase.Match` `planned` | `DM002.Suffix.Return.DefaultCase.Match` `planned` | `DM002.Suffix.Attribute.DefaultCase.Match` `planned` | `DM002.Suffix.SwitchArm.DefaultCase.Match` `planned` |
| substring | `DM002.Substring.Const.DefaultCase.Match` `planned` | `DM002.Substring.Comparison.DefaultCase.Match` `planned` | `DM002.Substring.MethodArgument.DefaultCase.Match` `planned` | `DM002.Substring.Return.DefaultCase.Match` `covered` | `DM002.Substring.Attribute.DefaultCase.Match` `covered` | `DM002.Substring.SwitchArm.DefaultCase.Match` `planned` |

### Default Case-Insensitive Matching: Negative Matrix

Expected result for every cell in this matrix: no `DM002` diagnostic.

| Pattern kind \ Context | Const field | Comparison | Method argument | Return value | Attribute argument | Switch arm |
| --- | --- | --- | --- | --- | --- | --- |
| exact | `DM002.Exact.Const.DefaultCase.NonMatch` `covered` | `DM002.Exact.Comparison.DefaultCase.NonMatch` `planned` | `DM002.Exact.MethodArgument.DefaultCase.NonMatch` `planned` | `DM002.Exact.Return.DefaultCase.NonMatch` `planned` | `DM002.Exact.Attribute.DefaultCase.NonMatch` `planned` | `DM002.Exact.SwitchArm.DefaultCase.NonMatch` `planned` |
| prefix | `DM002.Prefix.Const.DefaultCase.NonMatch` `planned` | `DM002.Prefix.Comparison.DefaultCase.NonMatch` `planned` | `DM002.Prefix.MethodArgument.DefaultCase.NonMatch` `planned` | `DM002.Prefix.Return.DefaultCase.NonMatch` `planned` | `DM002.Prefix.Attribute.DefaultCase.NonMatch` `planned` | `DM002.Prefix.SwitchArm.DefaultCase.NonMatch` `planned` |
| suffix | `DM002.Suffix.Const.DefaultCase.NonMatch` `planned` | `DM002.Suffix.Comparison.DefaultCase.NonMatch` `planned` | `DM002.Suffix.MethodArgument.DefaultCase.NonMatch` `planned` | `DM002.Suffix.Return.DefaultCase.NonMatch` `planned` | `DM002.Suffix.Attribute.DefaultCase.NonMatch` `planned` | `DM002.Suffix.SwitchArm.DefaultCase.NonMatch` `planned` |
| substring | `DM002.Substring.Const.DefaultCase.NonMatch` `planned` | `DM002.Substring.Comparison.DefaultCase.NonMatch` `planned` | `DM002.Substring.MethodArgument.DefaultCase.NonMatch` `planned` | `DM002.Substring.Return.DefaultCase.NonMatch` `planned` | `DM002.Substring.Attribute.DefaultCase.NonMatch` `planned` | `DM002.Substring.SwitchArm.DefaultCase.NonMatch` `planned` |

### Case-Sensitive Matching: Positive Matrix

Expected result for every cell in this matrix: `DM002` diagnostic reported when
pattern casing and literal casing are identical.

| Pattern kind \ Context | Const field | Comparison | Method argument | Return value | Attribute argument | Switch arm |
| --- | --- | --- | --- | --- | --- | --- |
| exact | `DM002.Exact.Const.CaseSensitive.Match` `planned` | `DM002.Exact.Comparison.CaseSensitive.Match` `planned` | `DM002.Exact.MethodArgument.CaseSensitive.Match` `planned` | `DM002.Exact.Return.CaseSensitive.Match` `planned` | `DM002.Exact.Attribute.CaseSensitive.Match` `planned` | `DM002.Exact.SwitchArm.CaseSensitive.Match` `planned` |
| prefix | `DM002.Prefix.Const.CaseSensitive.Match` `planned` | `DM002.Prefix.Comparison.CaseSensitive.Match` `planned` | `DM002.Prefix.MethodArgument.CaseSensitive.Match` `planned` | `DM002.Prefix.Return.CaseSensitive.Match` `planned` | `DM002.Prefix.Attribute.CaseSensitive.Match` `planned` | `DM002.Prefix.SwitchArm.CaseSensitive.Match` `planned` |
| suffix | `DM002.Suffix.Const.CaseSensitive.Match` `planned` | `DM002.Suffix.Comparison.CaseSensitive.Match` `planned` | `DM002.Suffix.MethodArgument.CaseSensitive.Match` `planned` | `DM002.Suffix.Return.CaseSensitive.Match` `planned` | `DM002.Suffix.Attribute.CaseSensitive.Match` `planned` | `DM002.Suffix.SwitchArm.CaseSensitive.Match` `planned` |
| substring | `DM002.Substring.Const.CaseSensitive.Match` `planned` | `DM002.Substring.Comparison.CaseSensitive.Match` `planned` | `DM002.Substring.MethodArgument.CaseSensitive.Match` `planned` | `DM002.Substring.Return.CaseSensitive.Match` `planned` | `DM002.Substring.Attribute.CaseSensitive.Match` `planned` | `DM002.Substring.SwitchArm.CaseSensitive.Match` `planned` |

### Case-Sensitive Matching: Negative Matrix

Expected result for every cell in this matrix: no `DM002` diagnostic when the
literal differs only by casing.

| Pattern kind \ Context | Const field | Comparison | Method argument | Return value | Attribute argument | Switch arm |
| --- | --- | --- | --- | --- | --- | --- |
| exact | `DM002.Exact.Const.CaseSensitive.NonMatch` `covered` | `DM002.Exact.Comparison.CaseSensitive.NonMatch` `planned` | `DM002.Exact.MethodArgument.CaseSensitive.NonMatch` `planned` | `DM002.Exact.Return.CaseSensitive.NonMatch` `planned` | `DM002.Exact.Attribute.CaseSensitive.NonMatch` `planned` | `DM002.Exact.SwitchArm.CaseSensitive.NonMatch` `planned` |
| prefix | `DM002.Prefix.Const.CaseSensitive.NonMatch` `planned` | `DM002.Prefix.Comparison.CaseSensitive.NonMatch` `planned` | `DM002.Prefix.MethodArgument.CaseSensitive.NonMatch` `planned` | `DM002.Prefix.Return.CaseSensitive.NonMatch` `planned` | `DM002.Prefix.Attribute.CaseSensitive.NonMatch` `planned` | `DM002.Prefix.SwitchArm.CaseSensitive.NonMatch` `planned` |
| suffix | `DM002.Suffix.Const.CaseSensitive.NonMatch` `planned` | `DM002.Suffix.Comparison.CaseSensitive.NonMatch` `planned` | `DM002.Suffix.MethodArgument.CaseSensitive.NonMatch` `planned` | `DM002.Suffix.Return.CaseSensitive.NonMatch` `planned` | `DM002.Suffix.Attribute.CaseSensitive.NonMatch` `planned` | `DM002.Suffix.SwitchArm.CaseSensitive.NonMatch` `planned` |
| substring | `DM002.Substring.Const.CaseSensitive.NonMatch` `planned` | `DM002.Substring.Comparison.CaseSensitive.NonMatch` `planned` | `DM002.Substring.MethodArgument.CaseSensitive.NonMatch` `planned` | `DM002.Substring.Return.CaseSensitive.NonMatch` `planned` | `DM002.Substring.Attribute.CaseSensitive.NonMatch` `planned` | `DM002.Substring.SwitchArm.CaseSensitive.NonMatch` `planned` |

### Ignored Content, Config, Severity, And Suppression

| ID | Mode | Expected | Analyzer corpus fixture | Package smoke fixture | State |
| --- | --- | --- | --- | --- | --- |
| `DM002-001` | interpolated hole | no diagnostic | `DM002/InterpolatedHole.cs` | `Samples/DM002/InterpolatedHole.cs` | `planned` |
| `DM002-002` | comments | no diagnostic | `DM002/CommentsAndDocumentationIgnored.cs` | `Samples/DM002/CommentsIgnored.cs` | `planned` |
| `DM002-003` | XML documentation | no diagnostic | `DM002/CommentsAndDocumentationIgnored.cs` | `Samples/DM002/XmlDocumentationIgnored.cs` | `planned` |
| `DM002-004` | missing config | no diagnostic | `DM002/MissingConfigNoDiagnostics.cs` | `Samples/DM002/MissingConfigNoDiagnostics.cs` | `planned` |
| `DM002-005` | disabled config | no diagnostic | `DM002/ExactMatchConstField.cs` | `Samples/DM002/DisabledConfigNoDiagnostics.cs` | `planned` |
| `DM002-006` | invalid severity | no diagnostic | `DM002/InvalidConfigNoDiagnostics.cs` | `Samples/DM002/InvalidSeverityNoDiagnostics.cs` | `planned` |
| `DM002-007` | severity `hidden` | hidden diagnostic | `DM002/SeverityHidden.cs` | `Samples/DM002/SeverityHidden.cs` | `planned` |
| `DM002-008` | severity `info` | info diagnostic | `DM002/SeverityFromConfig.cs` | `Samples/DM002/SeverityInfo.cs` | `planned` |
| `DM002-009` | severity `warning` | warning diagnostic | `DM002/PrefixMethodArgument.cs` | `Samples/DM002/SeverityWarning.cs` | `planned` |
| `DM002-010` | severity `error` | error diagnostic | `DM002/ExactMatchConstField.cs` | `Samples/DM002/SeverityError.cs` | `planned` |
| `DM002-011` | `#pragma` suppression | no diagnostic | `DM002/SuppressedLiteral.cs` | `Samples/DM002/SuppressedLiteral.cs` | `covered` |
| `DM002-012` | unsuppressed control for suppression sample | diagnostic | `DM002/SuppressedLiteral.cs` | `Samples/DM002/UnsuppressedLiteralControl.cs` | `covered` |

## Unsupported By Design In v1

| ID | Permutation | Reason | State |
| --- | --- | --- | --- |
| `UNSUPPORTED-001` | `DM001` on `static readonly` fields | deferred by PRD open question | `unsupported` |
| `UNSUPPORTED-002` | `DM002` inside interpolated string holes | explicitly excluded by requirements | `unsupported` |
| `UNSUPPORTED-003` | `DM002` on comments or XML doc text as diagnostics | analyzer is syntax-literal scoped only | `unsupported` |
| `UNSUPPORTED-004` | code-fix permutations for either rule | code fixes are out of scope for v1 | `unsupported` |
