namespace Roslyn.Lint.Tests.Commands;

using System.Text.Json.Nodes;

internal static class CliTestHost
{
    public static async Task<CliInvocationResult> InvokeAsync(params string[] args)
    {
        using var stdout = new StringWriter();
        using var stderr = new StringWriter();

        var exitCode = await CliApplication.RunAsync(args, stdout, stderr, CancellationToken.None);

        return new CliInvocationResult(exitCode, stdout.ToString(), stderr.ToString());
    }

    public static JsonObject ParseJsonObject(string json)
        => JsonNode.Parse(json)!.AsObject();
}
