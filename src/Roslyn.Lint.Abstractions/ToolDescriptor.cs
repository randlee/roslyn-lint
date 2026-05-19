namespace Roslyn.Lint.Abstractions;

public sealed record ToolDescriptor
{
    public ToolDescriptor(
        ToolId id,
        string displayName,
        string description,
        string packageName,
        IReadOnlyList<string>? commandFamilies,
        IReadOnlyList<string>? capabilities)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name is required.", nameof(displayName));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (string.IsNullOrWhiteSpace(packageName))
            throw new ArgumentException("Package name is required.", nameof(packageName));

        Id = id;
        DisplayName = displayName;
        Description = description;
        PackageName = packageName;
        CommandFamilies = commandFamilies?.ToArray() ?? [];
        Capabilities = capabilities?.ToArray() ?? [];
    }

    public ToolId Id { get; }

    public string DisplayName { get; }

    public string Description { get; }

    public string PackageName { get; }

    public IReadOnlyList<string> CommandFamilies { get; }

    public IReadOnlyList<string> Capabilities { get; }
}
