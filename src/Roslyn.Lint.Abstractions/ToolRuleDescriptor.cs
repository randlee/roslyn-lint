namespace Roslyn.Lint.Abstractions;

public sealed record ToolRuleDescriptor
{
    public ToolRuleDescriptor(
        ToolId toolId,
        string id,
        string title,
        string category,
        string defaultSeverity,
        bool isEnabledByDefault,
        string messageFormat,
        string? description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(category);
        ArgumentException.ThrowIfNullOrWhiteSpace(defaultSeverity);
        ArgumentException.ThrowIfNullOrWhiteSpace(messageFormat);

        ToolId = toolId;
        Id = id;
        Title = title;
        Category = category;
        DefaultSeverity = defaultSeverity;
        IsEnabledByDefault = isEnabledByDefault;
        MessageFormat = messageFormat;
        Description = description;
    }

    public ToolId ToolId { get; }

    public string Id { get; }

    public string Title { get; }

    public string Category { get; }

    public string DefaultSeverity { get; }

    public bool IsEnabledByDefault { get; }

    public string MessageFormat { get; }

    public string? Description { get; }
}
