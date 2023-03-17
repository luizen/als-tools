namespace AlsTools.Ui.Cli.CliOptions;

public partial class LocateOptions : CommonOptions
{
    [Option("scene-names", Group = "locate options", Min = 1, HelpText = "The scene names to locate projects by.")]
    public IReadOnlyCollection<string>? SceneNamesToLocate { get; set; }

    [Option("scene-annotations", Group = "locate options", Min = 1, HelpText = "The scene annotations to locate projects by.")]
    public IReadOnlyCollection<string>? SceneAnnotationsToLocate { get; set; }

    [Option("scene-tempos", Group = "locate options", Min = 1, HelpText = "The scene tempos to locate projects by.")]
    public IReadOnlyCollection<int>? SceneTemposToLocate { get; set; }
}