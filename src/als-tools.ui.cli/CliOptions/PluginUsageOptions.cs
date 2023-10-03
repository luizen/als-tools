namespace AlsTools.Ui.Cli.CliOptions;

[Verb("usage", HelpText = "List only used or unused plugins. This uses as input the plugin list extracted from PlugInfo app.")]
public class PluginUsageOptions : CommonOptions
{

    [Option("select", HelpText = "Whether to list only used or unused plugins (UsedOnly or UnusedOnly)", Default = PluginUsageSelection.UnusedOnly)]
    public PluginUsageSelection Select { get; set; }
}