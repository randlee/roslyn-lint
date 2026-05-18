; Shipped analyzer releases
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

## Release 0.1.0

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
DM001 | roslyn-lint.Organization | Warning | Released in Phase A v0.1.0 on 2026-05-18. DM001ConstantConsolidationAnalyzer enforces designated-file consolidation for public and internal constants.
DM002 | roslyn-lint.DomainBoundary | Error | Released in Phase A v0.1.0 on 2026-05-18. DM002ForbiddenStringLiteralAnalyzer blocks configured forbidden string literals from `.roslyn-lint` configuration.
