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

        var table = CreateSimpleConsoleTable("Number of tracks per project", ["Project", "Path", "Tracks count"]);

        foreach (var trackCount in tracksCountPerProject)
        {
            await Task.Run(() => table.AddRow(
                new Text(trackCount.ProjectName),
                new Text(trackCount.ProjectPath),
                new Text(trackCount.ItemsCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }

    private async Task PrintNumberOfPluginsPerProject(PrintStatisticsOptions options)
    {
        var projectsAndPluginCount = await liveProjectService.GetPluginsCountPerProject(options.IgnoreDisabledDevices);

        var table = CreateSimpleConsoleTable("Number of plugins per project", ["Project", "Path", "Plugins count"]);

        foreach (var pluginCount in projectsAndPluginCount)
        {
            await Task.Run(() => table.AddRow(
                new Text(pluginCount.ProjectName),
                new Text(pluginCount.ProjectPath),
                new Text(pluginCount.ItemsCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }

    private async Task PrintNumberOfStockDevicesPerProject(PrintStatisticsOptions options)
    {
        var projectsAndStockDevicesCount = await liveProjectService.GetStockDevicesCountPerProject(options.IgnoreDisabledDevices);

        var table = CreateSimpleConsoleTable("Number of stock devices per project", ["Project", "Path", "Stock devices count"]);

        foreach (var stockDevicesCount in projectsAndStockDevicesCount)
        {
            await Task.Run(() => table.AddRow(
                new Text(stockDevicesCount.ProjectName),
                new Text(stockDevicesCount.ProjectPath),
                new Text(stockDevicesCount.ItemsCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }

    private async Task PrintProjectsWithHighestPluginCount(PrintStatisticsOptions options)
    {
        var projectsAndPluginCount = await liveProjectService.GetProjectsWithHighestPluginsCount(options.Limit, options.IgnoreDisabledDevices);

        var table = CreateSimpleConsoleTable("Projects with highest plugin count", ["Project", "Path", "Plugins count"]);

        foreach (var pluginUsage in projectsAndPluginCount)
        {
            await Task.Run(() => table.AddRow(
                new Text(pluginUsage.ProjectName),
                new Text(pluginUsage.ProjectPath),
                new Text(pluginUsage.ItemsCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }

    private async Task PrintProjectsWithHighestTracksCount(PrintStatisticsOptions options)
    {
        var projectsAndTrackCount = await liveProjectService.GetProjectsWithHighestTracksCount(options.Limit);

        var table = CreateSimpleConsoleTable("Projects with highest track count", ["Project", "Path", "Tracks count"]);

        foreach (var p in projectsAndTrackCount)
        {
            await Task.Run(() => table.AddRow(
                new Text(p.ProjectName),
                new Text(p.ProjectPath),
                new Text(p.ItemsCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }

    private async Task PrintMostUsedStockDevices(PrintStatisticsOptions options)
    {

        var stockDevicesUsageCount = await liveProjectService.GetMostUsedStockDevices(options.Limit, options.IgnoreDisabledDevices);

        var table = CreateSimpleConsoleTable("Most used stock devices", ["Stock device name", "Usage count"], expand: false);

        foreach (var p in stockDevicesUsageCount)
        {
            await Task.Run(() => table.AddRow(
                new Text(p.DeviceName),
                new Text(p.UsageCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
    }

    private async Task PrintMostUsedPlugins(PrintStatisticsOptions options)
    {
        var pluginsUsageCount = await liveProjectService.GetMostUsedPlugins(options.Limit, options.IgnoreDisabledDevices);

        var table = CreateSimpleConsoleTable("Most used plugins", ["Plugin name", "Usage count"], expand: false);

        foreach (var p in pluginsUsageCount)
        {
            await Task.Run(() => table.AddRow(
                new Text(p.DeviceName),
                new Text(p.UsageCount.ToString())));
        }

        await Task.Run(() => AnsiConsole.Write(table));
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

    private Table CreateSimpleConsoleTable(string title, string[] columnNames, bool wrap = true, bool expand = true)
    {
        var paddedTitle = $" {title} ";
        var tableTitle = new TableTitle(paddedTitle, new Style(foreground: Color.Black, background: Color.SkyBlue1));
        var table = new Table().Title(tableTitle).Border(TableBorder.Rounded).BorderColor(Color.SkyBlue1);

        if (expand)
            table.Expand();

        foreach (var columnName in columnNames)
        {
            table.AddColumn(columnName, wrap ? null : c => c.NoWrap());
        }

        return table;
    }

}