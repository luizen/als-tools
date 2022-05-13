using System.Collections.Generic;
using CommandLine;

namespace AlsTools.CliOptions
{
    [Verb("locate", HelpText = "Locates projects containing given plugins by their names.")]
    class LocateOptions
    {
        [Option("plugin-names", Required = true, Min = 1, HelpText = "The plugin names to locate projects by.")]
        public IEnumerable<string> PluginsToLocate { get; set; }

        [Option("json-only", HelpText = "The output will be generated as clean JSON, without any other extra information.")]
        public bool JsonOnly { get; set; } = false;
    }
}