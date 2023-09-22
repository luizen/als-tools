namespace AlsTools.Ui.Cli.CliOptions;

[Verb("stats", HelpText = "Print statistics.")]
public class PrintStatisticsOptions : CommonOptions
{
    [Option("ignore-disabled", HelpText = "Ignores the devices that are directly disabled (turned off) or are within a disabled rack.", Default = false)]
    public bool IgnoreDisabledDevices { get; set; }
}