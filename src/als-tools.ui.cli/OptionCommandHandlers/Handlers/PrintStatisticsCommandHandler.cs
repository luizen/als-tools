using Spectre.Console;

namespace AlsTools.Ui.Cli.OptionCommandHandlers.Handlers;

public partial class PrintStatisticsCommandHandler : BaseCommandHandler, IOptionCommandHandler<PrintStatisticsOptions>
{
    private readonly ILogger<PrintStatisticsCommandHandler> logger;
    private readonly ILiveProjectAsyncService liveProjectService;

    public PrintStatisticsCommandHandler(ILogger<PrintStatisticsCommandHandler> logger,
        ILiveProjectAsyncService liveProjectService,
        IOptions<ParameterValuesOptions> parameterValuesOptions) : base(parameterValuesOptions)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
    }

    public async Task Execute(PrintStatisticsOptions options)
    {
        logger.LogDebug("Printing statistics...");

        var projects = await liveProjectService.GetAllProjectsAsync();
        var ignoreDisabledDevices = options.IgnoreDisabledDevices;

        SetAllOptionsIfAll(options);

        ExecuteIfOptionWasSet(options.CountProjects, () => logger.LogDebug(@"Total of projects: {@TotalOfProjects}", projects.Count));

        ExecuteIfOptionWasSet(options.TracksPerProject, () => PrintNumberOfTracksPerProject(projects, options));

        ExecuteIfOptionWasSet(options.StockDevicesPerProject, () => PrintNumberOfStockDevicesPerProject(projects, options));

        ExecuteIfOptionWasSet(options.PluginsPerProject, () => PrintNumberOfPluginsPerProject(projects, options));

        ExecuteIfOptionWasSet(options.MostUsedPlugins, () => PrintMostUsedPlugins(projects, options));

        ExecuteIfOptionWasSet(options.MostUsedStockDevice, () => PrintMostUsedStockDevices(projects, options));

        ExecuteIfOptionWasSet(options.ProjectsWithHighestPluginCount, () => PrintProjectsWithHighestPluginCount(projects, options));

        ExecuteIfOptionWasSet(options.ProjectsWithHighestTrackCount, () => PrintProjectsWithHighestTrackCount(projects, options));
    }

    private void PrintProjectsWithHighestTrackCount(IReadOnlyList<LiveProject> projects, PrintStatisticsOptions options)
    {
        var trackCountPerProject = projects.Select(project => new
        {
            ProjectName = project.Name,
            TrackCount = project.Tracks.Count()
        })
        .OrderByDescending(project => project.TrackCount)
        .Take(options.Limit)
        .ToList();

        PrintHeader($"Projects with highest track count");
        var table = CreateSimpleConsoleTable("Project name", "Tracks count");

        foreach (var trackCount in trackCountPerProject)
        {
            table.AddRow(
                new Text(trackCount.ProjectName),
                new Text(trackCount.TrackCount.ToString()));
        }

        AnsiConsole.Write(table);
    }

    private void PrintProjectsWithHighestPluginCount(IReadOnlyList<LiveProject> projects, PrintStatisticsOptions options)
    {
        var ignoreDisabledPredicate = GenerateIgnoreDisabledPredicate<PluginDevice>(options);

        var pluginCountPerProject = projects.Select(project => new
        {
            ProjectName = project.Name,
            PluginsCount = project.Tracks
                .SelectMany(track => track.Plugins)
                .Count(ignoreDisabledPredicate)
        })
        .OrderByDescending(project => project.PluginsCount)
        .Take(options.Limit)
        .ToList();

        PrintHeader($"Projects with highest plugin count");
        var table = CreateSimpleConsoleTable("Project name", "Plugins count");

        foreach (var pluginUsage in pluginCountPerProject)
        {
            table.AddRow(
                new Text(pluginUsage.ProjectName),
                new Text(pluginUsage.PluginsCount.ToString()));
        }

        AnsiConsole.Write(table);
    }

    private void PrintNumberOfPluginsPerProject(IReadOnlyList<LiveProject> projects, PrintStatisticsOptions options)
    {
        var ignoreDisabledPredicate = GenerateIgnoreDisabledPredicate<PluginDevice>(options);

        var projectsAndPluginsCount = projects.Select(project => new
        {
            ProjectName = project.Name,
            PluginCount = project.Tracks
                .Sum(track => track.Plugins.Where(ignoreDisabledPredicate).Count())

        })
        .OrderByDescending(x => x.PluginCount)
        .ToList();

        PrintHeader($"Number of plugins per project");
        var table = CreateSimpleConsoleTable("Project", "Plugins count");

        foreach (var p in projectsAndPluginsCount)
        {
            table.AddRow(
                new Text(p.ProjectName),
                new Text(p.PluginCount.ToString()));
        }

        AnsiConsole.Write(table);
    }


    private void PrintMostUsedStockDevices(IReadOnlyList<LiveProject> projects, PrintStatisticsOptions options)
    {
        var ignoreDisabledPredicate = GenerateIgnoreDisabledPredicate<StockDevice>(options);

        var stockDevicesUsageList = projects
           .SelectMany(project => project.Tracks
               .SelectMany(track => track.StockDevices.Where(ignoreDisabledPredicate))
               .GroupBy(device => device.Name)
               .Select(group => new
               {
                   StockDeviceName = group.Key,
                   UsageCount = group.Count()
               })
           )
           .GroupBy(deviceUsage => deviceUsage.StockDeviceName)
           .Select(group => new
           {
               StockDeviceName = group.Key,
               TotalUsageCount = group.Sum(deviceUsage => deviceUsage.UsageCount)
           })
           .OrderByDescending(deviceUsage => deviceUsage.TotalUsageCount)
           .Take(options.Limit)
           .ToList();

        PrintHeader($"Most used stock devices");
        var table = CreateSimpleConsoleTable("Stock device name", "Usage count");

        foreach (var usage in stockDevicesUsageList)
        {
            table.AddRow(
                new Text(usage.StockDeviceName),
                new Text(usage.TotalUsageCount.ToString()));
        }

        AnsiConsole.Write(table);
    }

    private void PrintMostUsedPlugins(IReadOnlyList<LiveProject> projects, PrintStatisticsOptions options)
    {
        var ignoreDisabledPredicate = GenerateIgnoreDisabledPredicate<PluginDevice>(options);

        var pluginsUsageList = projects
           .SelectMany(project => project.Tracks
               .SelectMany(track => track.Plugins.Where(ignoreDisabledPredicate))
               .GroupBy(plugin => plugin.Name)
               .Select(group => new
               {
                   PluginName = group.Key,
                   UsageCount = group.Count()
               })
           )
           .GroupBy(pluginUsage => pluginUsage.PluginName)
           .Select(group => new
           {
               PluginName = group.Key,
               TotalUsageCount = group.Sum(pluginUsage => pluginUsage.UsageCount)
           })
           .OrderByDescending(pluginUsage => pluginUsage.TotalUsageCount)
           .Take(options.Limit)
           .ToList();

        PrintHeader($"Most used plugins");
        var table = CreateSimpleConsoleTable("Plugin name", "Usage count");

        foreach (var pluginUsage in pluginsUsageList)
        {
            table.AddRow(
                new Text(pluginUsage.PluginName),
                new Text(pluginUsage.TotalUsageCount.ToString()));
        }

        AnsiConsole.Write(table);
    }

    private void PrintNumberOfStockDevicesPerProject(IReadOnlyList<LiveProject> projects, PrintStatisticsOptions options)
    {
        var ignoreDisabledPredicate = GenerateIgnoreDisabledPredicate<StockDevice>(options);

        var projectsAndStockDevicesCount = projects.Select(project => new
        {
            ProjectName = project.Name,
            StockDevicesCount = project.Tracks
                .Sum(track => track.StockDevices.Where(ignoreDisabledPredicate).Count())

        })
        .OrderByDescending(x => x.StockDevicesCount)
        .ToList();

        PrintHeader($"Number of stock devices per project");
        var table = CreateSimpleConsoleTable("Project", "Stock devices count");

        foreach (var p in projectsAndStockDevicesCount)
        {
            table.AddRow(
                new Text(p.ProjectName),
                new Text(p.StockDevicesCount.ToString()));
        }

        AnsiConsole.Write(table);
    }

    private async Task PrintNumberOfTracksPerProject_UsingDB(PrintStatisticsOptions options)
    {
        var projectsAndTrackCount = await liveProjectService.GetTracksCountPerProjectAsync();

        PrintHeader($"Number of tracks per project");
        var table = CreateSimpleConsoleTable("Project", "Track count");

        foreach (var p in projectsAndTrackCount)
        {
            table.AddRow(
                new Text(p.Name),
                new Text(p.Count.ToString()));
        }

        AnsiConsole.Write(table);
    }

    private void PrintNumberOfTracksPerProject(IReadOnlyList<LiveProject> projects, PrintStatisticsOptions options)
    {
        var projectsAndTrackCount = projects.Select(project => new
        {
            ProjectName = project.Name,
            TrackCount = project.Tracks.Count()
        })
        .OrderByDescending(x => x.TrackCount)
        .ToList();

        PrintHeader($"Number of tracks per project");
        var table = CreateSimpleConsoleTable("Project", "Track count");

        foreach (var p in projectsAndTrackCount)
        {
            table.AddRow(
                new Text(p.ProjectName),
                new Text(p.TrackCount.ToString()));
        }

        AnsiConsole.Write(table);
    }

    private void ExecuteIfOptionWasSet(bool optionValue, Action action)
    {
        if (optionValue)
            action();
    }

    private void SetAllOptionsIfAll(PrintStatisticsOptions options)
    {
        if (options.All)
        {
            options.PluginsPerProject =
                options.TracksPerProject =
                    options.CountProjects =
                        options.StockDevicesPerProject =
                            options.MostUsedPlugins =
                                options.MostUsedStockDevice =
                                    options.ProjectsWithHighestPluginCount =
                                        options.ProjectsWithHighestTrackCount = true;
        }
    }

    private Table CreateSimpleConsoleTable(string column1Name, string column2Name)
    {
        return new Table()
            .AddColumn(column1Name, c => c.NoWrap())
            .AddColumn(column2Name, c => c.NoWrap());
    }

    private void PrintHeader(string text)
    {
        Console.WriteLine(" ");
        Console.WriteLine("============================================================================================");
        Console.WriteLine(text);
        Console.WriteLine("============================================================================================");
        Console.WriteLine(" ");
    }

    private Func<TDeviceType, bool> GenerateIgnoreDisabledPredicate<TDeviceType>(PrintStatisticsOptions options) where TDeviceType : IDevice
    {
        Func<TDeviceType, bool> predicate = (TDeviceType device) => !options.IgnoreDisabledDevices || device.IsEnabled;

        return predicate;
    }
}