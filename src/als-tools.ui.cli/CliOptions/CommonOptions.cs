namespace AlsTools.Ui.Cli.CliOptions;

/// <summary>
/// Common options available for all verbs
/// </summary>
public abstract class CommonOptions
{
    [Option("log-level", HelpText = "The logging level (Verbose, Debug, Information, Warning, Error or Fatal)", Default = LoggingLevels.Information)]
    public LoggingLevels LoggingLevel { get; set; }
}