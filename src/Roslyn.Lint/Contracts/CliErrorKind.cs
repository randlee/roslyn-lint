namespace Roslyn.Lint.Contracts;

public enum CliErrorKind
{
    Validation,
    NotFound,
    InvalidState,
    Dependency,
    Internal,
}
