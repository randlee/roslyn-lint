namespace sc.lint.roslyn.demagic;

public static class DeMagicConstants
{
    public const string Dm001Id = "DM001";
    public const string Dm002Id = "DM002";

    public const string Dm001Category = "sc.lint.roslyn.organization";
    public const string Dm002Category = "sc.lint.roslyn.domainboundary";

    public const string Dm001Title = "Public/internal constant must be consolidated";
    public const string Dm002Title = "Forbidden string literal";

    public const string Dm001MessageFormat =
        "Constant '{0}' must be declared in '{1}' to keep shared constants consolidated";

    public const string Dm002MessageFormat =
        "String literal '{0}' matches forbidden pattern '{1}' and is not permitted in this codebase";

    public const string Dm001Description =
        "DM001 enforces designated-file consolidation for public and internal constants.";

    public const string Dm002Description =
        "DM002 blocks configured domain-specific string literals.";
}
