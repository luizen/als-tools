using CommandLine;

namespace AlsTools.CliOptions
{
    [Verb("list", HelpText = "List all projects stored in the als-tools database.")]
    public class ListOptions
    {
        [Option("json-only", HelpText = "The output will be generated as clean JSON, without any other extra information.")]
        public bool JsonOnly { get; set; } = false;
    }
}