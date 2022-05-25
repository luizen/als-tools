using CommandLine;

namespace AlsTools.CliOptions;

[Verb("list", HelpText = "List all projects stored in the als-tools database.")]
public class ListOptions : CommonOptions
{
}