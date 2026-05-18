namespace Roslyn.Lint.Contracts;

using System.Text.Json.Serialization;

public sealed record ViewResult
{
    public ViewResult(
        string target,
        IReadOnlyList<ViewToolResult>? tools = null,
        IReadOnlyList<ViewRuleResult>? rules = null)
    {
        Target = target;
        Tools = tools is { Count: > 0 } ? tools : null;
        Rules = rules is { Count: > 0 } ? rules : null;
    }

    public string Target { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ViewToolResult>? Tools { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ViewRuleResult>? Rules { get; }
}

public sealed record ViewToolResult(
    string Id,
    string DisplayName,
    string Description,
    string PackageName,
    IReadOnlyList<string> CommandFamilies,
    IReadOnlyList<string> Capabilities);

public sealed record ViewRuleResult(
    string Tool,
    string Id,
    string Title,
    string Category,
    string DefaultSeverity,
    bool IsEnabledByDefault,
    string MessageFormat,
    string? Description);
