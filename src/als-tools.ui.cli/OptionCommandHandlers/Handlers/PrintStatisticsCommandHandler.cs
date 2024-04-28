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
        var total = await liveProjectService.GetProjectsCount();

        logger.LogDebug(@"Total of projects: {@TotalOfProjects}", total);
    }

    private async Task PrintNumberOfTracksPerProject()
    {
        var tracksCountPerProject = await liveProjectService.GetTracksCountPerProject();

        PrintHeader($"Number of tracks per project");
        var table = CreateSimpleConsoleTable("Project", "Path", "Tracks count");

        foreach (var trackCount in tracksCountPerProject)
        {
            await Task.Run(() => table.AddRow(
                new Text(trackCount.ProjectName),
                new Text(trackCount.ProjectPath),
                new Text(trackCount.TracksCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }

    private async Task PrintNumberOfPluginsPerProject(PrintStatisticsOptions options)
    {
        var projectsAndPluginCount = await liveProjectService.GetPluginsCountPerProject(options.IgnoreDisabledDevices);

        PrintHeader($"Number of plugins per project");
        var table = CreateSimpleConsoleTable("Project", "Path", "Plugins count");

        foreach (var pluginCount in projectsAndPluginCount)
        {
            await Task.Run(() => table.AddRow(
                new Text(pluginCount.ProjectName),
                new Text(pluginCount.ProjectPath),
                new Text(pluginCount.PluginsCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }

    private async Task PrintNumberOfStockDevicesPerProject(PrintStatisticsOptions options)
    {
        var projectsAndStockDevicesCount = await liveProjectService.GetStockDevicesCountPerProject(options.IgnoreDisabledDevices);

        PrintHeader($"Number of stock devices per project");
        var table = CreateSimpleConsoleTable("Project", "Path", "Stock devices count");

        foreach (var stockDevicesCount in projectsAndStockDevicesCount)
        {
            await Task.Run(() => table.AddRow(
                new Text(stockDevicesCount.ProjectName),
                new Text(stockDevicesCount.ProjectPath),
                new Text(stockDevicesCount.StockDevicesCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }

    private async Task PrintProjectsWithHighestPluginCount(PrintStatisticsOptions options)
    {
        var projectsAndPluginCount = await liveProjectService.GetProjectsWithHighestPluginsCount(options.Limit, options.IgnoreDisabledDevices);

        PrintHeader($"Projects with highest plugin count");
        var table = CreateSimpleConsoleTable("Project", "Path", "Plugins count");

        foreach (var pluginUsage in projectsAndPluginCount)
        {
            await Task.Run(() => table.AddRow(
                new Text(pluginUsage.ProjectName),
                new Text(pluginUsage.ProjectPath),
                new Text(pluginUsage.PluginsCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }

    private async Task PrintProjectsWithHighestTracksCount(PrintStatisticsOptions options)
    {
        var projectsAndTrackCount = await liveProjectService.GetProjectsWithHighestTracksCount(options.Limit);

        PrintHeader($"Projects with highest track count");
        var table = CreateSimpleConsoleTable("Project", "Path", "Tracks count");

        foreach (var p in projectsAndTrackCount)
        {
            await Task.Run(() => table.AddRow(
                new Text(p.ProjectName),
                new Text(p.ProjectPath),
                new Text(p.TracksCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }

    private async Task PrintMostUsedStockDevices(PrintStatisticsOptions options)
    {

        var stockDevicesUsageCount = await liveProjectService.GetMostUsedStockDevices(options.Limit, options.IgnoreDisabledDevices);

        PrintHeader($"Most used stock devices");

        var table = CreateSimpleConsoleTable("Stock device name", "Usage count");

        foreach (var p in stockDevicesUsageCount)
        {
            await Task.Run(() => table.AddRow(
                new Text(p.StockDeviceName),
                new Text(p.UsageCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }

    private async Task PrintMostUsedPlugins(PrintStatisticsOptions options)
    {
        var pluginsUsageCount = await liveProjectService.GetMostUsedPlugins(options.Limit, options.IgnoreDisabledDevices);

        PrintHeader($"Most used plugins");

        var table = CreateSimpleConsoleTable("Plugin name", "Usage count");

        foreach (var p in pluginsUsageCount)
        {
            await Task.Run(() => table.AddRow(
                new Text(p.PluginName),
                new Text(p.UsageCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }


    private void ExecuteIfOptionWasSet(bool optionValue, Action action)
    {
        if (optionValue)
            action();
    }

    private async Task ExecuteIfOptionWasSetAsync(bool optionValue, Func<Task> asyncAction)
    {
        if (optionValue)
            await asyncAction();
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

    private Table CreateSimpleConsoleTable(string column1Name, string column2Name, string column3Name)
    {
        return new Table()
            .AddColumn(column1Name, c => c.NoWrap())
            .AddColumn(column2Name, c => c.NoWrap())
            .AddColumn(column3Name, c => c.NoWrap());
    }

    private void PrintHeader(string text)
    {
        Console.WriteLine(" ");
        Console.WriteLine("============================================================================================");
        Console.WriteLine(text);
        Console.WriteLine("============================================================================================");
        Console.WriteLine(" ");
    }
}