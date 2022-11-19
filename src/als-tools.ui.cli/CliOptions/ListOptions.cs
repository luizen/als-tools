namespace AlsTools.Ui.Cli.CliOptions;

[Verb("list", HelpText = "List all projects stored in the als-tools database.")]
public class ListOptions : CommonOptions
{
    public override bool IsEmpty => false;
}