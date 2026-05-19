namespace Roslyn.Lint.Tests.Commands;

public sealed record CliInvocationResult(int ExitCode, string StdOut, string StdErr);
