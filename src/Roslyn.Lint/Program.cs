using System.Reflection;
using Roslyn.Lint.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();

var version = Assembly.GetExecutingAssembly()
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
    ?? Assembly.GetExecutingAssembly().GetName().Version?.ToString()
    ?? "0.0.0";

app.Configure(config =>
{
    config.SetApplicationName("roslyn-lint");
    config.SetApplicationVersion(version);

    config.AddCommand<LintCommand>("lint")
        .WithDescription("Analyze C# files or directories for code quality issues")
        .WithExample("lint", "src/MyFile.cs")
        .WithExample("lint", "--format", "json", "src/")
        .WithExample("lint", "--include", "*.cs", "--exclude", "**/obj/**", "src/");
});

return await app.RunAsync(args);
