namespace AlsTools.Ui.Cli.CliOptions;

public partial class LocateOptions : CommonOptions
{
    [Option("project-names", Group = "locate options", Min = 1, HelpText = "The project names to locate projects by.")]
    public IReadOnlyCollection<string>? ProjectNamesToLocate { get; set; }

    [Option("project-paths", Group = "locate options", Min = 1, HelpText = "The project paths to locate projects by.")]
    public IReadOnlyCollection<string>? ProjectPathsToLocate { get; set; }

    [Option("project-creators", Group = "locate options", Min = 1, HelpText = "The project creators to locate projects by.")]
    public IReadOnlyCollection<string>? ProjectCreatorsToLocate { get; set; }

    [Option("project-minor-versions", Group = "locate options", Min = 1, HelpText = "The project minor versions to locate projects by.")]
    public IReadOnlyCollection<string>? ProjectMinorVersionsToLocate { get; set; }

    [Option("project-major-versions", Group = "locate options", Min = 1, HelpText = "The project major versions to locate projects by.")]
    public IReadOnlyCollection<string>? ProjectMajorVersionsToLocate { get; set; }

    [Option("project-tempos", Group = "locate options", Min = 1, HelpText = "The project tempos (BPM) to locate projects by.")]
    public IReadOnlyCollection<double>? ProjectTemposToLocate { get; set; }
}