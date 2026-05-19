namespace sc.lint.roslyn.demagic.packagesmoke.samples.dm002;

internal sealed class SuffixComparison
{
    public bool IsLegacy(string value)
    {
        return value == "legacy-atm";
    }
}
