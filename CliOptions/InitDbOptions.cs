using CommandLine;

namespace AlsTools.CliOptions;

[Verb("initdb", HelpText = @"Initialize the als-tools database with information extracted from Live sets, either from files or folders.")]
public class InitDbOptions : CommonOptions
{
    [Option("folders", Required = true, SetName = "folders", Min = 1, HelpText = "The root folders to look for Live Sets, recursively including children folders. This option is mutually exclusive with the --files option.")]
    public IReadOnlyCollection<string> Folders { get; set; }

    [Option("include-backups", SetName = "folders", Default = false, HelpText = "Set it to true to include backup Live Sets.")]
    public bool IncludeBackups { get; set; }


    [Option("files", Required = true, SetName = "files", Min = 1, HelpText = "The files to extract Live Set information from. This option is mutually exclusive with the --folders option.")]
    public IReadOnlyCollection<string> Files { get; set; }
}