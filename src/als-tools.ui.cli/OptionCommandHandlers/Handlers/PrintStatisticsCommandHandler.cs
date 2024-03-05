﻿using System.Diagnostics.CodeAnalysis;
using Spectre.Console;

namespace AlsTools.Ui.Cli.OptionCommandHandlers.Handlers;

public class PrintStatisticsCommandHandler : BaseCommandHandler, IOptionCommandHandler<PrintStatisticsOptions>
{
    private readonly ILogger<PrintStatisticsCommandHandler> logger;
    private readonly ILiveProjectAsyncService liveProjectService;
    private readonly ProjectsAndPluginsPrinter projectsAndPluginsPrinter;
    // private readonly IOptions<PlugScanningOptions> plugScanningOptions;

    public PrintStatisticsCommandHandler(ILogger<PrintStatisticsCommandHandler> logger, ILiveProjectAsyncService liveProjectService, ProjectsAndPluginsPrinter projectsAndPluginsPrinter, IOptions<ParameterValuesOptions> parameterValuesOptions /*, IOptions<PlugScanningOptions> plugScanningOptions*/) : base(parameterValuesOptions)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
        this.projectsAndPluginsPrinter = projectsAndPluginsPrinter;
        // this.plugScanningOptions = plugScanningOptions;
    }

    public async Task Execute(PrintStatisticsOptions options)
    {
        logger.LogDebug("Printing statistics...");

        var projects = await liveProjectService.GetAllProjectsAsync();
        // var skipPlugins = plugScanningOptions.Value.SkipPlugins;
        // var pluginsToSkip = plugScanningOptions.Value.PluginsToSkip;
        var ignoreDisabledDevices = options.IgnoreDisabledDevices;

        // Func<PluginDevice, bool> predicate = (PluginDevice plugin) => plugin.Format == PluginFormat.VST2;


        // Func<PluginDevice, bool> predicate = (PluginDevice plugin) =>
        //     plugin.Format == PluginFormat.VST2 &&
        //     (!skipPlugins || pluginsToSkip.Any(x => x.PluginFormat == plugin.Format && x.PluginName == plugin.Name)) &&
        //     (!ignoreDisabledDevices || plugin.IsEnabled);

        // var allProjectsWithVst2Plugings = projects
        //     .Select(project => new ProjectStatsInfo()
        //     {
        //         ProjectName = project.Name,
        //         ProjectPath = project.Path,
        //         Vst2PluginsCount = project.Tracks
        //             .SelectMany(track => track.Plugins)
        //             .Where(predicate)
        //             .Select(plugin => plugin.Name)
        //             .Count(),
        //         Vst2PluginNames = project.Tracks
        //             .SelectMany(track => track.Plugins)
        //             .Where(predicate)
        //             .Select(plugin => plugin.Name)
        //             .ToList(),
        //         DistinctVst2PluginsCount = project.Tracks
        //             .SelectMany(track => track.Plugins)
        //             .Where(predicate)
        //             .Select(plugin => plugin.Name)
        //             .Distinct()
        //             .Count(),
        //         DistinctVst2PluginNames = project.Tracks
        //             .SelectMany(track => track.Plugins)
        //             .Where(predicate)
        //             .Select(plugin => plugin.Name)
        //             .Distinct()
        //             .ToList(),
        //         Vst2PluginsStats = project.Tracks
        //             .SelectMany(track => track.Plugins)
        //             .Where(predicate)
        //             .GroupBy(plugin => plugin.Name)
        //             .SelectMany(group =>
        //                 group.Select(plugin => new PluginStatsInfo()
        //                 {
        //                     PluginName = plugin.Name,
        //                     PluginCount = group.Count()
        //                 })
        //             )
        //             .Distinct(new PluginStatsInfoEqualityComparerByPluginName())
        //             .ToList()
        //     })
        //     .Where(x => x.DistinctVst2PluginsCount > 0);

        // .OrderByDescending(projectInfo => projectInfo.DistinctVst2PluginsCount)
        // .ToList();

        SetAllOptionsIfAll(options);

        ExecuteIfTrue(options.CountProjects, () => logger.LogDebug(@"Total of projects: {@TotalOfProjects}", projects.Count));

        ExecuteIfTrue(options.TracksPerProject, () => PrintNumberOfTracksPerProject(projects, options));

        ExecuteIfTrue(options.StockDevicesPerProject, () => PrintNumberOfStockDevicesPerProject(projects, options));

        ExecuteIfTrue(options.MostUsedPlugins, () => PrintMostUsedPlugins(projects, options));

        ExecuteIfTrue(options.MostUsedStockDevice, () => PrintMostUsedStockDevices(projects, options));

        if (options.PluginsPerProject)
        {

        }

        if (options.ProjectsWithHighestPluginCount)
        {

        }

        if (options.ProjectsWithHighestTrackCount)
        {

        }

        if (options.StockDevicesPerProject)
        {

        }

        if (options.TracksPerProject)
        {

        }

        // int max = 10;

        // PrintAllProjectsWithVst2PluginsComplete(allProjectsWithVst2Plugings, true);
        // PrintProjectsWithMostVst2Plugins(max, allProjectsWithVst2Plugings);
        // PrintProjectsWithMostGivenPluginVst2Plugin(max, "Decapitator", projects);
        // PrintProjectsWithMostGivenPluginVst2Plugin(max, "minimoog V Original", projects);
        // PrintProjectsWithMostGivenPluginVst2Plugin(max, "Maschine 2", projects);
        // PrintProjectsWithMostGivenPluginVst2Plugin(max, "Komplete Kontrol", projects);
        // PrintProjectsWithMostGivenPluginsListVst2Plugin(max, projects, "Decapitator", "minimoog V Original", "Maschine 2", "Komplete Kontrol");
        // PrintProjectsWithLeastVst2Plugins(max, projects);
        // PrintVst2PluginsTable(projects);

        // logger.LogDebug(@"Total of projects: {@TotalOfProjects}", projects.Count);
    }

    private void PrintMostUsedStockDevices(IReadOnlyList<LiveProject> projects, PrintStatisticsOptions options)
    {
        Func<StockDevice, bool> ignoreDisabledPredicate = (StockDevice device) => !options.IgnoreDisabledDevices || device.IsEnabled;

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
        Func<PluginDevice, bool> ignoreDisabledPredicate = (PluginDevice plugin) => !options.IgnoreDisabledDevices || plugin.IsEnabled;

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
        Func<StockDevice, bool> ignoreDisabledPredicate = (StockDevice device) => !options.IgnoreDisabledDevices || device.IsEnabled;

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

    private void ExecuteIfTrue(bool optionValue, Action action)
    {
        if (optionValue)
            action();
    }

    private void PrintVst2PluginsTable(IReadOnlyList<LiveProject> projects)
    {
        var pluginUsageList = projects
            .SelectMany(project => project.Tracks
                .SelectMany(track => track.Plugins)
                .Where(plugin => plugin.Format == PluginFormat.VST2)
                .GroupBy(plugin => plugin.Name)
                .Select(group => new PluginUsageInfo
                {
                    PluginName = group.Key,
                    UsageCount = group.Count(),
                    ProjectNames = new List<string> { project.Name }
                })
            )
            .GroupBy(pluginUsage => pluginUsage.PluginName)
            .Select(group => new PluginUsageInfo
            {
                PluginName = group.Key,
                UsageCount = group.Sum(pluginUsage => pluginUsage.UsageCount),
                ProjectNames = group.SelectMany(pluginUsage => pluginUsage.ProjectNames).Distinct().ToList()
            })
            .OrderBy(pluginUsage => pluginUsage.UsageCount)
            .ToList();

        PrintHeader("VST2 PLUGINS TABLE");

        Console.WriteLine("VST2 Plugin Name | Total Projects Used In");
        Console.WriteLine("---------------------------------------");

        foreach (var pluginUsage in pluginUsageList)
        {
            Console.WriteLine($"{pluginUsage.PluginName,-17} | {pluginUsage.ProjectNames.Count,-24}");
        }
    }

    private void PrintProjectsWithMostGivenPluginVst2Plugin(int max, string pluginName, IReadOnlyList<LiveProject> projects)
    {
        var topProjectsWithMostVst2Plugings = projects
                            .Select(project => new
                            {
                                ProjectName = project.Name,
                                ProjectPath = project.Path,
                                PluginCount = project.Tracks
                                    .SelectMany(track => track.Plugins)
                                    .Where(plugin => plugin.Format == PluginFormat.VST2 && plugin.Name == pluginName)
                                    .Select(plugin => plugin.Name)
                                    .Count()
                            })
                            .OrderByDescending(projectInfo => projectInfo.PluginCount)
                            .Take(max)
                            .ToList();

        PrintHeader($"Top {max} projects with most {pluginName} VST2 plugins");
        foreach (var projectVST2Info in topProjectsWithMostVst2Plugings)
        {
            Console.WriteLine($"Project Name: {projectVST2Info.ProjectName}");
            Console.WriteLine($"Project Path: {projectVST2Info.ProjectPath}");
            Console.WriteLine($"Distinct {pluginName} VST2 Plugin Count: {projectVST2Info.PluginCount}");
            Console.WriteLine();
        }
    }

    private void PrintProjectsWithMostGivenPluginsListVst2Plugin(int max, IReadOnlyList<LiveProject> projects, params string[] pluginNames)
    {
        var topProjectsWithMostVst2Plugings = projects
                            .Select(project => new
                            {
                                ProjectName = project.Name,
                                ProjectPath = project.Path,
                                PluginCount = project.Tracks
                                    .SelectMany(track => track.Plugins)
                                    .Where(plugin => plugin.Format == PluginFormat.VST2 && pluginNames.Contains(plugin.Name))
                                    .Select(plugin => plugin.Name)
                                    .Count()
                            })
                            .OrderByDescending(projectInfo => projectInfo.PluginCount)
                            .Take(max)
                            .ToList();

        PrintHeader($"Top {max} projects with most {pluginNames} VST2 plugins");
        foreach (var projectVST2Info in topProjectsWithMostVst2Plugings)
        {
            Console.WriteLine($"Project Name: {projectVST2Info.ProjectName}");
            Console.WriteLine($"Project Path: {projectVST2Info.ProjectPath}");
            Console.WriteLine($"{pluginNames} VST2 Plugin Count: {projectVST2Info.PluginCount}");
            Console.WriteLine();
        }
    }

    private void PrintProjectsWithMostVst2Plugins(int max, IEnumerable<ProjectStatsInfo> allProjectsWithVst2Plugings)
    {
        // var topProjectsWithMostVst2Plugings = projects
        //                     .Select(project => new
        //                     {
        //                         ProjectName = project.Name,
        //                         ProjectPath = project.Path,
        //                         DistinctPluginCount = project.Tracks
        //                             .SelectMany(track => track.Plugins)
        //                             .Where(plugin => plugin.Format == PluginFormat.VST2)
        //                             .Select(plugin => plugin.Name)
        //                             .Distinct()
        //                             .Count(),
        //                         PluginNames = project.Tracks
        //                             .SelectMany(track => track.Plugins)
        //                             .Where(plugin => plugin.Format == PluginFormat.VST2)
        //                             .Select(plugin => plugin.Name)
        //                             .Distinct()
        //                             .ToList()
        //                     })
        //                     .OrderByDescending(projectInfo => projectInfo.DistinctPluginCount)
        //                     .Take(max)
        //                     .ToList();

        PrintHeader($"Top {max} projects with most VST2 plugins");

        var table = CreateConsoleTableForPluginStats();

        foreach (var projectStatsInfo in allProjectsWithVst2Plugings.OrderByDescending(projectInfo => projectInfo.Vst2PluginsCount).Take(max).ToList())
        {
            PrintProjectStatsInfo(projectStatsInfo, table, true);
        }
    }

    private Table CreateSimpleConsoleTable(string column1Name, string column2Name)
    {
        return new Table()
            .HideHeaders()
            .AddColumn(column1Name, c => c.NoWrap())
            .AddColumn(column2Name, c => c.NoWrap());
    }

    private Table CreateConsoleTableForPluginStats()
    {
        return CreateSimpleConsoleTable("Plugin name", "Count");
    }

    private Table CreateConsoleTableForFullProjectStats()
    {
        return new Table()
            .Expand()
            .Border(TableBorder.Rounded)
            .AddColumn("Project")
            .AddColumn("VST2s\r\nCount", c => c.Alignment(Justify.Center))
            .AddColumn("Distinct\r\nVST2s\r\nCount", c => c.Alignment(Justify.Center))
            .AddColumn("Distinct VST2s Names", c => c.NoWrap = false)
            .AddColumn("VST2s Stats");
    }

    private void PrintAllProjectsWithVst2PluginsComplete(IEnumerable<ProjectStatsInfo> allProjectsWithVst2Plugings, bool printProjectsAsTable = true)
    {
        PrintHeader($"All projects with VST2 plugins (complete)");

        Table? projectsTable = printProjectsAsTable ? CreateConsoleTableForFullProjectStats() : null;

        // foreach (var projectStatsInfo in allProjectsWithVst2Plugings.OrderByDescending(projectInfo => projectInfo.DistinctVst2PluginsCount).ToList())
        foreach (var projectStatsInfo in allProjectsWithVst2Plugings.OrderByDescending(projectInfo => projectInfo.Vst2PluginsCount).ToList())
        {
            PrintProjectStatsInfo(projectStatsInfo, projectsTable, true, printProjectsAsTable);
        }

        if (projectsTable != null)
        {
            AnsiConsole.Write(projectsTable);
        }
    }

    private void PrintProjectStatsInfo(ProjectStatsInfo projectStatsInfo, Table? projectsTable, bool printVst2PluginStats = true, bool printProjectsAsTable = true)
    {
        Table? pluginsTable = null;
        if (printVst2PluginStats)
        {
            pluginsTable = CreateConsoleTableForPluginStats();
            projectStatsInfo.Vst2PluginsStats.ForEach(plugin => pluginsTable.AddRow(plugin.PluginName, $"[fuchsia]{plugin.PluginCount}[/]"));
        }

        if (printProjectsAsTable && projectsTable is not null)
        {
            // var projectPanel = new Panel(new TextPath(projectStatsInfo.ProjectPath))
            var projectPanel = new Panel($"[link]{projectStatsInfo.ProjectPath}[/]")
            {
                Header = new PanelHeader($":musical_keyboard: [gold1]{projectStatsInfo.ProjectName}[/]"),
            };

            projectsTable.AddRow(
                projectPanel,
                new Markup($"[darkorange]{projectStatsInfo.Vst2PluginsCount}[/]"),
                new Text(projectStatsInfo.DistinctVst2PluginsCount.ToString()),
                new Text(string.Join(", ", projectStatsInfo.DistinctVst2PluginNames)),
                pluginsTable != null ? pluginsTable : Text.Empty);
        }
        else
        {
            Console.WriteLine($"Project name: {projectStatsInfo.ProjectName}");
            Console.WriteLine($"Project path: {projectStatsInfo.ProjectPath}");
            Console.WriteLine($"VST2 plugins count: {projectStatsInfo.Vst2PluginsCount}");
            Console.WriteLine($"Distinct VST2 plugins count: {projectStatsInfo.DistinctVst2PluginsCount}");
            Console.WriteLine($"Distinct VST2 plugin names: {string.Join(", ", projectStatsInfo.DistinctVst2PluginNames)}");
            Console.WriteLine("VST2 plugins stats table:");

            if (printVst2PluginStats && pluginsTable != null)
            {
                AnsiConsole.Write(pluginsTable);
            }
        }
    }

    private void PrintProjectsWithLeastVst2Plugins(int max, IReadOnlyList<LiveProject> projects)
    {
        var topProjectsWithLeastVst2Plugings = projects
                            .Select(project => new
                            {
                                ProjectName = project.Name,
                                ProjectPath = project.Path,
                                DistinctPluginCount = project.Tracks
                                    .SelectMany(track => track.Plugins)
                                    .Where(plugin => plugin.Format == PluginFormat.VST2)
                                    .Select(plugin => plugin.Name)
                                    .Distinct()
                                    .Count(),
                                PluginNames = project.Tracks
                                    .SelectMany(track => track.Plugins)
                                    .Where(plugin => plugin.Format == PluginFormat.VST2)
                                    .Select(plugin => plugin.Name)
                                    .Distinct()
                                    .ToList()
                            })
                            .Where(projectInfo => projectInfo.DistinctPluginCount > 0)
                            .OrderBy(projectInfo => projectInfo.DistinctPluginCount)
                            .Take(max)
                            .ToList();

        PrintHeader($"Top {max} projects with least VST2 plugins");
        foreach (var projectVST2Info in topProjectsWithLeastVst2Plugings)
        {
            Console.WriteLine($"Project Name: {projectVST2Info.ProjectName}");
            Console.WriteLine($"Project Path: {projectVST2Info.ProjectPath}");
            Console.WriteLine($"Distinct VST2 Plugin Count: {projectVST2Info.DistinctPluginCount}");
            Console.WriteLine($"VST2 Plugin Names: {string.Join(", ", projectVST2Info.PluginNames)}");
            Console.WriteLine();
        }
    }

    private void PrintHeader(string text)
    {
        Console.WriteLine(" ");
        Console.WriteLine("============================================================================================");
        Console.WriteLine(text);
        Console.WriteLine("============================================================================================");
        Console.WriteLine(" ");
    }

    private void PrintProjectsUsingVst2WavesPlugins(int max, IReadOnlyList<LiveProject> projects)
    {
        // "WaveShell1-VST 14.0."
        var projs = projects
            .Select(project => new
            {
                ProjectName = project.Name,
                ProjectPath = project.Path,
                Plugins = project.Tracks
                    .SelectMany(track => track.Plugins)
                    .Where(plugin => plugin.Format == PluginFormat.VST2 && plugin.Path.Contains("WaveShell"))
                    .Select(plugin => new
                    {
                        PluginName = plugin.Name,
                        PluginPath = plugin.Path
                    })
                    .Distinct()
                    .ToList()
            })
            .Where(x => x.Plugins.Any())
            .Take(max)
            .ToList();

        Console.WriteLine($"Top {max} projects containing VST2 Waves plugins");
        foreach (var proj in projs)
        {
            Console.WriteLine($"Project Name: {proj.ProjectName}");
            Console.WriteLine($"Project Path: {proj.ProjectPath}");
            Console.WriteLine($"VST2 Plugin Names:");
            foreach (var plug in proj.Plugins)
            {
                Console.WriteLine($"\t{plug.PluginName} --> {plug.PluginPath}");
            }

            Console.WriteLine();
        }
    }
}

internal class PluginUsageInfo
{
    public PluginUsageInfo()
    {
        ProjectNames = new List<string>();
    }

    public string PluginName { get; set; } = string.Empty;
    public int UsageCount { get; set; } = 0;
    public List<string> ProjectNames { get; set; }
}

internal class ProjectStatsInfo
{
    public ProjectStatsInfo()
    {
        DistinctVst2PluginNames = new List<string>();
        Vst2PluginNames = new List<string>();
        Vst2PluginsStats = new List<PluginStatsInfo>();
    }

    public string ProjectName { get; set; } = string.Empty;
    public string ProjectPath { get; set; } = string.Empty;
    public int Vst2PluginsCount { get; set; }
    public List<string> Vst2PluginNames { get; set; }
    public int DistinctVst2PluginsCount { get; set; }
    public List<string> DistinctVst2PluginNames { get; set; }
    public List<PluginStatsInfo> Vst2PluginsStats { get; set; }
}

internal class PluginStatsInfo
{
    public string PluginName { get; set; } = string.Empty;

    public int PluginCount { get; set; } = 0;
}

internal class PluginStatsInfoEqualityComparerByPluginName : IEqualityComparer<PluginStatsInfo>
{
    public bool Equals(PluginStatsInfo? x, PluginStatsInfo? y)
    {
        return x!.PluginName.Equals(y!.PluginName);
    }

    public int GetHashCode([DisallowNull] PluginStatsInfo obj)
    {
        return obj.PluginName.GetHashCode();
    }
}