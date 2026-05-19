namespace sc.lint.roslyn.abstractions;

public readonly record struct ToolId
{
    public ToolId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Tool id is required.", nameof(value));

        Value = value.Trim();
    }

    public string Value { get; }

    public override string ToString() => Value;
}
