namespace AlsTools.Ui.Cli.CliOptions;

[Verb("list", HelpText = "List all projects, or plugin names only, stored in the als-tools database.")]
public class ListOptions : CommonOptions
{

    [Option("plugin-names-only", HelpText = "Whether to only display the plugin names found in all projects", Default = false)]
    public bool PluginNamesOnly { get; set; }

    public override bool IsEmpty => false;
}