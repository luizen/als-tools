using AlsTools.Core.ValueObjects;

namespace AlsTools.Ui.Cli.CliOptions;
public partial class LocateOptions : CommonOptions
{
    [Option("plugin-names", Group = "locate options", Min = 1, HelpText = "The plugin names to locate projects by.")]
    public IReadOnlyCollection<string>? PluginNamesToLocate { get; set; }

    [Option("plugin-formats", Group = "locate options", Min = 1, HelpText = "The plugin formats to locate projects by.")]
    public IReadOnlyCollection<PluginFormat>? PluginFormatsToLocate { get; set; }
}