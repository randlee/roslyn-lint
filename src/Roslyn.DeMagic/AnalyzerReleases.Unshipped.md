; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
DM001 | roslyn-lint.Organization | Warning | Not yet implemented; release metadata retained pending approved declaration-scope implementation
DM002 | roslyn-lint.DomainBoundary | Error | DM002ForbiddenStringLiteralAnalyzer: Detects forbidden string literals from `.roslyn-lint` configuration
