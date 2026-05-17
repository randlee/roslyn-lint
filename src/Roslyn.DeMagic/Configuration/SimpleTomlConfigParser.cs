namespace Roslyn.DeMagic.Configuration;

using System.Collections.Immutable;
using Roslyn.DeMagic.Patterns;

public sealed class SimpleTomlConfigParser : ITomlConfigParser
{
    public bool TryParse(string content, out DeMagicConfig config, out ImmutableArray<string> errors)
    {
        var errorList = new List<string>();
        var values = new Dictionary<string, Dictionary<string, object?>>(StringComparer.OrdinalIgnoreCase);

        string? currentSection = null;
        string? pendingArrayKey = null;
        List<string>? pendingArrayItems = null;

        var lines = content.Replace("\r\n", "\n").Split('\n');

        for (var index = 0; index < lines.Length; index++)
        {
            var rawLine = lines[index];
            var line = StripComment(rawLine).Trim();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (pendingArrayKey is not null)
            {
                if (line == "]")
                {
                    EnsureSection(values, currentSection!)[pendingArrayKey] = pendingArrayItems!.ToImmutableArray();
                    pendingArrayKey = null;
                    pendingArrayItems = null;
                    continue;
                }

                if (!TryParseArrayItem(line, out var item))
                {
                    errorList.Add($"Invalid array item for key '{pendingArrayKey}' on line {index + 1}.");
                    continue;
                }

                pendingArrayItems!.Add(item);
                continue;
            }

            if (line.StartsWith("[", StringComparison.Ordinal) && line.EndsWith("]", StringComparison.Ordinal))
            {
                currentSection = line.Substring(1, line.Length - 2).Trim();
                EnsureSection(values, currentSection);
                continue;
            }

            if (currentSection is null)
            {
                errorList.Add($"Key/value pair outside a section on line {index + 1}.");
                continue;
            }

            var separatorIndex = line.IndexOf('=');
            if (separatorIndex < 0)
            {
                errorList.Add($"Missing '=' on line {index + 1}.");
                continue;
            }

            var key = line.Substring(0, separatorIndex).Trim();
            var valueText = line.Substring(separatorIndex + 1).Trim();

            if (valueText == "[")
            {
                pendingArrayKey = key;
                pendingArrayItems = [];
                continue;
            }

            if (!TryParseScalar(valueText, out var scalar))
            {
                errorList.Add($"Unsupported value for key '{key}' on line {index + 1}.");
                continue;
            }

            EnsureSection(values, currentSection)[key] = scalar;
        }

        if (pendingArrayKey is not null)
            errorList.Add($"Array value for key '{pendingArrayKey}' was not closed.");

        config = BuildConfig(values, errorList);
        errors = errorList.ToImmutableArray();
        return errorList.Count == 0;
    }

    private static Dictionary<string, object?> EnsureSection(
        Dictionary<string, Dictionary<string, object?>> values,
        string section)
    {
        if (!values.TryGetValue(section, out var sectionValues))
        {
            sectionValues = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            values[section] = sectionValues;
        }

        return sectionValues;
    }

    private static string StripComment(string line)
    {
        var inQuotes = false;

        for (var index = 0; index < line.Length; index++)
        {
            var current = line[index];
            if (current == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (!inQuotes && current == '#')
                return line.Substring(0, index);
        }

        return line;
    }

    private static bool TryParseArrayItem(string line, out string item)
    {
        item = string.Empty;

        var trimmed = StripComment(line).Trim().TrimEnd(',');
        return TryParseQuotedString(trimmed, out item);
    }

    private static bool TryParseScalar(string valueText, out object? scalar)
    {
        scalar = null;

        if (TryParseQuotedString(valueText, out var stringValue))
        {
            scalar = stringValue;
            return true;
        }

        if (bool.TryParse(valueText, out var boolValue))
        {
            scalar = boolValue;
            return true;
        }

        return false;
    }

    private static bool TryParseQuotedString(string text, out string value)
    {
        value = string.Empty;

        if (text.Length < 2 || text[0] != '"' || text[text.Length - 1] != '"')
            return false;

        value = text.Substring(1, text.Length - 2);
        return true;
    }

    private static DeMagicConfig BuildConfig(
        Dictionary<string, Dictionary<string, object?>> values,
        List<string> errors)
    {
        var dm001Values = values.TryGetValue("dm001", out var firstRule)
            ? firstRule
            : new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        var dm002Values = values.TryGetValue("dm002", out var secondRule)
            ? secondRule
            : new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

        return new DeMagicConfig(
            new Dm001Options(
                Enabled: GetBool(dm001Values, "enabled", defaultValue: false, errors),
                Severity: GetSeverity(dm001Values, "severity", ConfiguredSeverity.Warning, errors),
                DesignatedFile: GetString(dm001Values, "designated_file", required: false, errors),
                DesignatedClass: GetString(dm001Values, "designated_class", required: false, errors)),
            new Dm002Options(
                Enabled: GetBool(dm002Values, "enabled", defaultValue: false, errors),
                Severity: GetSeverity(dm002Values, "severity", ConfiguredSeverity.Warning, errors),
                ForbiddenPatterns: GetForbiddenPatternArray(dm002Values, "forbidden", errors),
                CaseSensitive: GetBool(dm002Values, "case_sensitive", defaultValue: false, errors)));
    }

    private static bool GetBool(
        Dictionary<string, object?> section,
        string key,
        bool defaultValue,
        List<string> errors)
    {
        if (!section.TryGetValue(key, out var value))
            return defaultValue;

        if (value is bool boolValue)
            return boolValue;

        errors.Add($"Key '{key}' must be a boolean.");
        return defaultValue;
    }

    private static string? GetString(
        Dictionary<string, object?> section,
        string key,
        bool required,
        List<string> errors)
    {
        if (!section.TryGetValue(key, out var value))
        {
            if (required)
                errors.Add($"Missing required key '{key}'.");

            return null;
        }

        if (value is string stringValue)
            return stringValue;

        errors.Add($"Key '{key}' must be a string.");
        return null;
    }

    private static ImmutableArray<ForbiddenPattern> GetForbiddenPatternArray(
        Dictionary<string, object?> section,
        string key,
        List<string> errors)
    {
        if (!section.TryGetValue(key, out var value))
            return ImmutableArray<ForbiddenPattern>.Empty;

        if (value is ImmutableArray<string> arrayValue)
            return arrayValue.Select(item => new ForbiddenPattern(item)).ToImmutableArray();

        errors.Add($"Key '{key}' must be an array of strings.");
        return ImmutableArray<ForbiddenPattern>.Empty;
    }

    private static ConfiguredSeverity GetSeverity(
        Dictionary<string, object?> section,
        string key,
        ConfiguredSeverity defaultValue,
        List<string> errors)
    {
        var rawSeverity = GetString(section, key, required: false, errors);
        if (rawSeverity is null)
            return defaultValue;

        return rawSeverity.ToLowerInvariant() switch
        {
            "hidden" => ConfiguredSeverity.Hidden,
            "info" => ConfiguredSeverity.Info,
            "warning" => ConfiguredSeverity.Warning,
            "error" => ConfiguredSeverity.Error,
            _ => AddSeverityError(rawSeverity, errors, defaultValue),
        };
    }

    private static ConfiguredSeverity AddSeverityError(
        string rawSeverity,
        List<string> errors,
        ConfiguredSeverity defaultValue)
    {
        errors.Add($"Unsupported severity '{rawSeverity}'.");
        return defaultValue;
    }
}
