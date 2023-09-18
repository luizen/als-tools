
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;

namespace AlsTools.Ui.Cli;

public class PrintStatisticsCommandHandler : IOptionCommandHandler<PrintStatisticsOptions>
{
    private readonly ILogger<PrintStatisticsCommandHandler> logger;
    private readonly ILiveProjectAsyncService liveProjectService;
    private readonly ProjectsAndPluginsPrinter projectsAndPluginsPrinter;

    public PrintStatisticsCommandHandler(ILogger<PrintStatisticsCommandHandler> logger, ILiveProjectAsyncService liveProjectService, ProjectsAndPluginsPrinter projectsAndPluginsPrinter)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
        this.projectsAndPluginsPrinter = projectsAndPluginsPrinter;
    }

    public async Task Execute(PrintStatisticsOptions options)
    {
        logger.LogDebug("Printing statistics...");

        var projects = await liveProjectService.GetAllProjectsAsync();
        // var skipPlugins = plugScanningOptions.Value.SkipPlugins;
        // var pluginsToSkip = plugScanningOptions.Value.PluginsToSkip;

        var allProjectsWithVst2Plugings = projects
            .Select(project => new ProjectStatsInfo()
            {
                ProjectName = project.Name,
                ProjectPath = project.Path,
                Vst2PluginsCount = project.Tracks
                    .SelectMany(track => track.Plugins)
                    // .Where(plugin => plugin.Format == PluginFormat.VST2 && (!skipPlugins || pluginsToSkip.Any(x => x.PluginFormat == PluginFormat.VST2 && x.PluginName == plugin.Name)))
                    .Where(plugin => plugin.Format == PluginFormat.VST2)
                    .Select(plugin => plugin.Name)
                    .Count(),
                Vst2PluginNames = project.Tracks
                    .SelectMany(track => track.Plugins)
                    // .Where(plugin => plugin.Format == PluginFormat.VST2 && (!skipPlugins || pluginsToSkip.Any(x => x.PluginFormat == PluginFormat.VST2 && x.PluginName == plugin.Name)))
                    .Where(plugin => plugin.Format == PluginFormat.VST2)
                    .Select(plugin => plugin.Name)
                    .ToList(),
                DistinctVst2PluginsCount = project.Tracks
                    .SelectMany(track => track.Plugins)
                    // .Where(plugin => plugin.Format == PluginFormat.VST2 && (!skipPlugins || pluginsToSkip.Any(x => x.PluginFormat == PluginFormat.VST2 && x.PluginName == plugin.Name)))
                    .Where(plugin => plugin.Format == PluginFormat.VST2)
                    .Select(plugin => plugin.Name)
                    .Distinct()
                    .Count(),
                DistinctVst2PluginNames = project.Tracks
                    .SelectMany(track => track.Plugins)
                    // .Where(plugin => plugin.Format == PluginFormat.VST2 && (!skipPlugins || pluginsToSkip.Any(x => x.PluginFormat == PluginFormat.VST2 && x.PluginName == plugin.Name)))
                    .Where(plugin => plugin.Format == PluginFormat.VST2)
                    .Select(plugin => plugin.Name)
                    .Distinct()
                    .ToList(),
                Vst2PluginsStats = project.Tracks
                    .SelectMany(track => track.Plugins)
                    // .Where(plugin => plugin.Format == PluginFormat.VST2 && (!skipPlugins || pluginsToSkip.Any(x => x.PluginFormat == PluginFormat.VST2 && x.PluginName == plugin.Name)))
                    .Where(plugin => plugin.Format == PluginFormat.VST2)
                    .GroupBy(plugin => plugin.Name)
                    .SelectMany(group =>
                        group.Select(plugin => new PluginStatsInfo()
                        {
                            PluginName = plugin.Name,
                            PluginCount = group.Count()
                        })
                    )
                    .Distinct(new PluginStatsInfoEqualityComparerByPluginName())
                    .ToList()
            })
            .Where(x => x.DistinctVst2PluginsCount > 0);
        // .OrderByDescending(projectInfo => projectInfo.DistinctVst2PluginsCount)
        // .ToList();

        int max = 10;

        PrintAllProjectsWithVst2PluginsComplete(allProjectsWithVst2Plugings, true);
        // PrintProjectsWithMostVst2Plugins(max, allProjectsWithVst2Plugings);
        // PrintProjectsWithMostGivenPluginVst2Plugin(max, "Decapitator", projects);
        // PrintProjectsWithMostGivenPluginVst2Plugin(max, "minimoog V Original", projects);
        // PrintProjectsWithMostGivenPluginVst2Plugin(max, "Maschine 2", projects);
        // PrintProjectsWithMostGivenPluginVst2Plugin(max, "Komplete Kontrol", projects);
        // PrintProjectsWithMostGivenPluginsListVst2Plugin(max, projects, "Decapitator", "minimoog V Original", "Maschine 2", "Komplete Kontrol");
        // PrintProjectsWithLeastVst2Plugins(max, projects);
        // PrintVst2PluginsTable(projects);

        logger.LogDebug(@"Total of projects: {@TotalOfProjects}", projects.Count);
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

    private Table CreateConsoleTableForPluginStats()
    {
        return new Table()
            .HideHeaders()
            .AddColumn("Plugin name", c => c.NoWrap())
            .AddColumn("Count");
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