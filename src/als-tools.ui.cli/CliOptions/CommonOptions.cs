namespace AlsTools.Ui.Cli.CliOptions;

/// <summary>
/// Common options available for all verbs
/// </summary>
public abstract class CommonOptions : ICliOptions
{
    [Option("log-level", HelpText = "The logging level (Verbose, Debug, Information, Warning, Error or Fatal)", Default = LoggingLevels.Information)]
    public LoggingLevels LoggingLevel { get; set; }

    [Option("from-config", HelpText = "Reads the parameters from the app settings config file (appsettings.json) rather then the command line", Default = false)]
    public bool FromConfigFile { get; set; }
}