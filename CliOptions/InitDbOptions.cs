
using System.Collections.Generic;
using CommandLine;

namespace AlsTools.CliOptions
{
    [Verb("initdb", HelpText = "Initialize the als-tools database with information extracted from Live sets")]
    public class InitDbOptions
    {
        [Option("folders", Required = true, SetName = "folders", Min = 1)]
        public IEnumerable<string> Folders { get; set; }

        [Option("include-backups", SetName = "folders", Default = false)]
        public bool IncludeBackups { get; set; }


        [Option("files", Required = true, SetName = "files", Min = 1)]
        public IEnumerable<string> Files { get; set; }
    }
}