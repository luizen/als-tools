namespace AlsTools.Ui.Cli.CliOptions;

[Verb("stats", HelpText = "Print statistics.")]
public class PrintStatisticsOptions : CommonOptions
{
    public const uint DefaultLimitValue = 10;

    [Option("ignore-disabled", HelpText = "When calculating statistics, ignores the devices that are directly disabled (turned off) or are within a disabled rack.", Default = false)]
    public bool IgnoreDisabledDevices { get; set; }

    [Option("all", SetName = "StatsOptions", HelpText = "Prints all available statistics")]
    public bool All { get; set; } = false;

    [Option("count-projects", SetName = "StatsOptions", Min = 1, HelpText = "Shows the total number of projects stored in the als-tools database.")]
    public bool CountProjects { get; set; }

    [Option("tracks-per-project", SetName = "StatsOptions", Min = 1, HelpText = "Shows all project names and the number of tracks for each.")]
    public bool TracksPerProject { get; set; }

    [Option("plugins-per-project", SetName = "StatsOptions", Min = 1, HelpText = "Shows all project names and the number of plugins for each.")]
    public bool PluginsPerProject { get; set; }

    [Option("stock-devices-per-project", SetName = "StatsOptions", Min = 1, HelpText = "Shows all project names and the number of stock devices for each.")]
    public bool StockDevicesPerProject { get; set; }

    [Option("most-used-plugins", SetName = "StatsOptions", Min = 1, HelpText = "Shows a list of the top most used plugins. Use the --limit option to specify the number of top items to be displayed.")]
    public bool MostUsedPlugins { get; set; }

    [Option("most-used-stock-device", SetName = "StatsOptions", Min = 1, HelpText = "Shows a list of the top most used stock devices. Use the --limit option to specify the number of top items to be displayed.")]
    public bool MostUsedStockDevice { get; set; }

    [Option("projets-with-more-plugins", SetName = "StatsOptions", Min = 1, HelpText = "Shows a list of the top projects containing the higher count of plugins. Use the --limit option to specify the number of top items to be displayed.")]
    public bool ProjectsWithHighestPluginCount { get; set; }

    [Option("projets-with-more-tracks", SetName = "StatsOptions", Min = 1, HelpText = "Shows a list of the top projects containing the higher count of tracks. Use the --limit option to specify the number of top items to be displayed.")]
    public bool ProjectsWithHighestTrackCount { get; set; }

    [Option("limit", HelpText = "The number of items to be displayed by the stats. E.g.: the top <limit> most used plugins")]
    public uint Limit { get; set; } = DefaultLimitValue;
}