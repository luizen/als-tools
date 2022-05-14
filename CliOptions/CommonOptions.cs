using CommandLine;

namespace AlsTools.CliOptions
{
    /// <summary>
    /// Common options available for all verbs
    /// </summary>
    public abstract class CommonOptions
    {
        [Option("log-level", HelpText = "The logging level. Default is Information", Default = LoggingLevels.Information)]
        public LoggingLevels LoggingLevel { get; set; }
    }
}
