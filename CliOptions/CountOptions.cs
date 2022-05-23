using CommandLine;

namespace AlsTools.CliOptions;

[Verb("count", HelpText = "Returns the total of projects stored in the als-tools database.")]
public class CountOptions : CommonOptions
{
}
