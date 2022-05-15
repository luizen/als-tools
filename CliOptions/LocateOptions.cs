using System.Collections.Generic;
using CommandLine;

namespace AlsTools.CliOptions
{
    [Verb("locate", HelpText = "Locates projects containing given plugins by their names.")]
    public class LocateOptions : CommonOptions
    {
        [Option("plugin-names", Required = true, Min = 1, HelpText = "The plugin names to locate projects by.")]
        public IEnumerable<string> PluginsToLocate { get; set; }
    }
}