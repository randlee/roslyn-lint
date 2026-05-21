#pragma warning disable DM002
namespace sc.lint.roslyn.abstractions;

public static class ScLintRoslynConstants
{
    public static class Suite
    {
        public static readonly string CliName = "sc-lint-roslyn";
        public static readonly string SolutionFileName = "sc-lint-roslyn.sln";
        public static readonly string ConfigDirectoryName = ".sc-lint-roslyn";
    }

    public static class Tools
    {
        public static readonly string DeMagicValue = "demagic";
        public static readonly ToolId DeMagicId = new(DeMagicValue);
        public static readonly string DeMagicModuleName = "sc.lint.roslyn.demagic";
        public static readonly string DeMagicAnalyzerAssemblyName = "sc-lint-roslyn-demagic";
    }

    public static class Commands
    {
        public static readonly string Version = "version";
        public static readonly string Check = "check";
        public static readonly string Clippy = "clippy";
        public static readonly string Ci = "ci";
        public static readonly string LintDemagic = "lint.demagic";
        public static readonly string LintFast = "lint.fast";
        public static readonly string LintFull = "lint.full";
        public static readonly string LintCi = "lint.ci";
        public static readonly string ViewTools = "view.tools";
        public static readonly string ViewRules = "view.rules";

        public static readonly string FastName = "fast";
        public static readonly string FullName = "full";
        public static readonly string CiName = "ci";
        public static readonly string ToolsName = "tools";
        public static readonly string RulesName = "rules";
    }
}
#pragma warning restore DM002
