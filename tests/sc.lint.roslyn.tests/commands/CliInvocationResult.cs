namespace sc.lint.roslyn.tests.commands;

public sealed record CliInvocationResult(int ExitCode, string StdOut, string StdErr);
