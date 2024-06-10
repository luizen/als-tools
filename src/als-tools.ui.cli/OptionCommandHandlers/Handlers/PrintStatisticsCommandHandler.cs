namespace AlsTools.Ui.Cli.OptionCommandHandlers.Handlers;

public partial class PrintStatisticsCommandHandler : BaseCommandHandler, IOptionCommandHandler<PrintStatisticsOptions>
{
    private readonly ILogger<PrintStatisticsCommandHandler> logger;
    private readonly ILiveProjectAsyncService liveProjectService;
    private readonly ConsoleTablePrinter consolePrinter;

    public PrintStatisticsCommandHandler(
        ILogger<PrintStatisticsCommandHandler> logger,
        ILiveProjectAsyncService liveProjectService,
        IOptions<ParameterValuesOptions> parameterValuesOptions,
        ConsoleTablePrinter consolePrinter) : base(parameterValuesOptions)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
        this.consolePrinter = consolePrinter;
    }

    public async Task Execute(PrintStatisticsOptions options)
    {
        logger.LogDebug("Printing statistics...");

        SetAllOptionsIfAll(options);

        await ExecuteIfOptionWasSetAsync(options.CountProjects, () => PrintTotalOfProjects());
        await ExecuteIfOptionWasSetAsync(options.TracksPerProject, () => PrintNumberOfTracksPerProject());
        await ExecuteIfOptionWasSetAsync(options.PluginsPerProject, () => PrintNumberOfPluginsPerProject(options));
        await ExecuteIfOptionWasSetAsync(options.StockDevicesPerProject, () => PrintNumberOfStockDevicesPerProject(options));
        await ExecuteIfOptionWasSetAsync(options.MostUsedPlugins, () => PrintMostUsedPlugins(options));
        await ExecuteIfOptionWasSetAsync(options.MostUsedStockDevice, () => PrintMostUsedStockDevices(options));
        await ExecuteIfOptionWasSetAsync(options.ProjectsWithHighestTrackCount, () => PrintProjectsWithHighestTracksCount(options));
        await ExecuteIfOptionWasSetAsync(options.ProjectsWithHighestPluginCount, () => PrintProjectsWithHighestPluginCount(options));
    }

    private async Task PrintTotalOfProjects()
    {
        var total = await liveProjectService.CountProjectsAsync();

        await consolePrinter.PrintResults(total, "Total of projects", ["Projects", "Total"], (p) =>
        {
            return [string.Empty, p.ToString()];
        }, expand: false);
    }

    private async Task PrintNumberOfTracksPerProject()
    {
        var tracksCountPerProject = await liveProjectService.GetTracksCountPerProject();

        await consolePrinter.PrintResults(tracksCountPerProject, "Number of tracks per project", ["Project", "Path", "Tracks count"], (p) =>
        {
            return [p.ProjectName, p.ProjectPath, p.ItemsCount.ToString()];
        });
    }

    private async Task PrintNumberOfPluginsPerProject(PrintStatisticsOptions options)
    {
        var projectsAndPluginCount = await liveProjectService.GetPluginsCountPerProject(options.IgnoreDisabledDevices);

        await consolePrinter.PrintResults(projectsAndPluginCount, "Number of plugins per project", ["Project", "Path", "Plugins count"], (p) =>
        {
            return [p.ProjectName, p.ProjectPath, p.ItemsCount.ToString()];
        });
    }

    private async Task PrintNumberOfStockDevicesPerProject(PrintStatisticsOptions options)
    {
        var projectsAndStockDevicesCount = await liveProjectService.GetStockDevicesCountPerProject(options.IgnoreDisabledDevices);

        await consolePrinter.PrintResults(projectsAndStockDevicesCount, "Number of stock devices per project", ["Project", "Path", "Stock devices count"], (p) =>
        {
            return [p.ProjectName, p.ProjectPath, p.ItemsCount.ToString()];
        });
    }

    private async Task PrintProjectsWithHighestPluginCount(PrintStatisticsOptions options)
    {
        var projectsAndPluginCount = await liveProjectService.GetProjectsWithHighestPluginsCount(options.Limit, options.IgnoreDisabledDevices);

        await consolePrinter.PrintResults(projectsAndPluginCount, "Projects with highest plugin count", ["Project", "Path", "Plugins count"], (p) =>
        {
            return [p.ProjectName, p.ProjectPath, p.ItemsCount.ToString()];
        });
    }

    private async Task PrintProjectsWithHighestTracksCount(PrintStatisticsOptions options)
    {
        var projectsAndTrackCount = await liveProjectService.GetProjectsWithHighestTracksCount(options.Limit);

        await consolePrinter.PrintResults(projectsAndTrackCount, "Projects with highest track count", ["Project", "Path", "Tracks count"], (p) =>
        {
            return [p.ProjectName, p.ProjectPath, p.ItemsCount.ToString()];
        });
    }

    private async Task PrintMostUsedStockDevices(PrintStatisticsOptions options)
    {
        var stockDevicesUsageCount = await liveProjectService.GetMostUsedStockDevices(options.Limit, options.IgnoreDisabledDevices);

        await consolePrinter.PrintResults(stockDevicesUsageCount, "Most used stock devices", ["Stock device name", "Usage count"], (p) =>
        {
            return [p.DeviceName, p.UsageCount.ToString()];
        },
        expand: false);
    }

    private async Task PrintMostUsedPlugins(PrintStatisticsOptions options)
    {
        var pluginsUsageCount = await liveProjectService.GetMostUsedPlugins(options.Limit, options.IgnoreDisabledDevices);

        await consolePrinter.PrintResults(pluginsUsageCount, "Most used plugins", ["Plugin name", "Usage count"], (p) =>
        {
            return [p.DeviceName, p.UsageCount.ToString()];
        },
        expand: false);
    }
}