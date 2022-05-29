namespace AlsTools.Ui.Cli.CliOptions;

[Verb("locate", HelpText = "Locates projects containing given plugins by their names.")]
public class LocateOptions : CommonOptions
{
    [Option("plugin-names", Required = true, Min = 1, HelpText = "The plugin names to locate projects by.")]
    public IReadOnlyCollection<string> PluginsToLocate { get; set; } = new List<string>();
}