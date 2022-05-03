using System.Collections.Generic;
using CommandLine;

namespace AlsTools.CliOptions
{
    [Verb("locate", HelpText = "Locates projects containing given plugins by their names")]
    class LocateOptions
    {
        [Option("plugin-names", Required = true, Min = 1)]
        public IEnumerable<string> PluginsToLocate { get; set; }
    }
}