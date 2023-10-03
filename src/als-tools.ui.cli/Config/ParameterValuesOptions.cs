namespace AlsTools.Ui.Cli;

public class ParameterValuesOptions
{
    public bool Enabled { get; set; }

    public CommonOptions? Common { get; set; }

    public CountOptions? Count { get; set; }

    public InitDbOptions? InitDb { get; set; }

    public ListOptions? ListOptions { get; set; }

    public LocateOptions? Locate { get; set; }

    public PluginUsageOptions? PluginUsage { get; set; }

    public PrintStatisticsOptions? Stats { get; set; }
}
