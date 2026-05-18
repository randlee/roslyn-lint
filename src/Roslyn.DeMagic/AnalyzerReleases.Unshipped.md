; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
DM001 | roslyn-lint.Organization | Warning | DM001ConstantConsolidationAnalyzer: Enforces designated-file consolidation for public and internal constants
DM002 | roslyn-lint.DomainBoundary | Error | DM002ForbiddenStringLiteralAnalyzer: Detects forbidden string literals from `.roslyn-lint` configuration
