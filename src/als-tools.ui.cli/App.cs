namespace AlsTools.Ui.Cli;

public class App
{
    private readonly ILogger<App> logger;
    private readonly ILiveProjectAsyncService liveProjectService;
    private readonly IOptions<PlugInfoOptions> plugInfoOptions;

    public App(ILogger<App> logger, ILiveProjectAsyncService liveProjectService, IOptions<PlugInfoOptions> plugInfoOptions)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
        this.plugInfoOptions = plugInfoOptions;
    }

    public async Task Run(ParserResult<object> parserResult)
    {
        logger.LogDebug("Starting application...");

        await parserResult.WithParsedAsync<InitDbOptions>(options => RunInitDb(options));
        await parserResult.WithParsedAsync<CountOptions>(options => RunCount(options));
        await parserResult.WithParsedAsync<ListOptions>(options => RunList(options));
        await parserResult.WithParsedAsync<PluginUsageOptions>(options => RunPluginUsage(options));
        await parserResult.WithParsedAsync<LocateOptions>(options => RunLocate(options));
        await parserResult.WithNotParsedAsync(errors => { throw new CommandLineParseException(errors); });
    }

    private async Task RunPluginUsage(PluginUsageOptions options)
    {
        logger.LogDebug("Listing {@UsedOrUnusedPluginsOnly} plugins only...", options.Select == PluginUsageSelection.UsedOnly ? "used" : "unused");

        var availablePlugins = GetAvailablePluginsFromPlugInfo();
        IReadOnlyList<PluginDevice> plugins = await liveProjectService.GetPluginUsageResults(availablePlugins, options.Select);

        var pluginNamesAndFormats = plugins.Select(plugin => new
        {
            PluginName = plugin.Name,
            PluginFormat = plugin.Format.ToString()
        });

        var json = JsonSerializer.Serialize<IEnumerable<dynamic>>(pluginNamesAndFormats, new JsonSerializerOptions { WriteIndented = true });
        await Console.Out.WriteLineAsync(json);
    }

    private IList<PluginDevice> GetAvailablePluginsFromPlugInfo()
    {
        List<string> vst2Plugins = File.ReadLines(plugInfoOptions.Value.InputFilePath)
            .Where(line => plugInfoOptions.Value.PluginPathOptions.Vst2Paths.Any(path => line.StartsWith(path)))
            .Select(line => Path.GetFileNameWithoutExtension(line))
            .ToList();

        List<string> vst3Plugins = File.ReadLines(plugInfoOptions.Value.InputFilePath)
            .Where(line => plugInfoOptions.Value.PluginPathOptions.Vst3Paths.Any(path => line.StartsWith(path)))
            .Select(line => Path.GetFileNameWithoutExtension(line))
            .ToList();

        List<string> auPlugins = File.ReadLines(plugInfoOptions.Value.InputFilePath)
            .Where(line => plugInfoOptions.Value.PluginPathOptions.AudioUnitPaths.Any(path => line.StartsWith(path)))
            .Select(line => Path.GetFileNameWithoutExtension(line))
            .ToList();

        IList<PluginDevice> plugins = new List<PluginDevice>();

        foreach (var pluginName in vst2Plugins)
        {
            plugins.Add(new PluginDevice(DeviceSort.Unknown, PluginFormat.VST2)
            {
                Name = pluginName
            });
        }

        foreach (var pluginName in vst3Plugins)
        {
            plugins.Add(new PluginDevice(DeviceSort.Unknown, PluginFormat.VST3)
            {
                Name = pluginName
            });
        }

        foreach (var pluginName in auPlugins)
        {
            plugins.Add(new PluginDevice(DeviceSort.Unknown, PluginFormat.AU)
            {
                Name = pluginName
            });
        }

        return plugins;
    }

    private async Task RunInitDb(InitDbOptions options)
    {
        logger.LogDebug("Initializing database...");

        int count = 0;
        if (options.Files.Any())
            count = await liveProjectService.InitializeDbFromFilesAsync(options.Files);
        else
            count = await liveProjectService.InitializeDbFromFoldersAsync(options.Folders, options.IncludeBackups);

        await Console.Out.WriteLineAsync($"\nTotal of projects loaded into DB: {count}");
    }

    private async Task RunCount(CountOptions options)
    {
        logger.LogDebug("Counting projects...");

        int count = await liveProjectService.CountProjectsAsync();

        await Console.Out.WriteLineAsync($"\nTotal of projects in the DB: {count}");
    }

    private async Task RunList(ListOptions options)
    {
        logger.LogDebug("Listing projects...");

        var projects = await liveProjectService.GetAllProjectsAsync();
        await PrintProjectsAndPlugins(projects);
        // PrintStatistics(projects);

        logger.LogDebug(@"Total of projects: {@TotalOfProjects}", projects.Count);
    }

    private void PrintStatistics(IReadOnlyList<LiveProject> projects)
    {
        int max = 500;

        // PrintProjectsWithMostVst2Plugins(max, projects);
        // PrintProjectsWithLeastVst2Plugins(max, projects);
        PrintVst2PluginsTable(projects);
        // PrintProjectsUsingVst2WavesPlugins(max, projects);
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

        Console.WriteLine("VST2 Plugin Name | Total Projects Used In");
        Console.WriteLine("---------------------------------------");

        foreach (var pluginUsage in pluginUsageList)
        {
            Console.WriteLine($"{pluginUsage.PluginName,-17} | {pluginUsage.ProjectNames.Count,-24}");
        }
    }

    private void PrintProjectsWithMostVst2Plugins(int max, IReadOnlyList<LiveProject> projects)
    {
        var topProjectsWithMostVst2Plugings = projects
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
                            .OrderByDescending(projectInfo => projectInfo.DistinctPluginCount)
                            .Take(max)
                            .ToList();

        Console.WriteLine($"Top {max} projects with most VST2 plugins");
        foreach (var projectVST2Info in topProjectsWithMostVst2Plugings)
        {
            Console.WriteLine($"Project Name: {projectVST2Info.ProjectName}");
            Console.WriteLine($"Project Path: {projectVST2Info.ProjectPath}");
            Console.WriteLine($"Distinct VST2 Plugin Count: {projectVST2Info.DistinctPluginCount}");
            Console.WriteLine($"VST2 Plugin Names: {string.Join(", ", projectVST2Info.PluginNames)}");
            Console.WriteLine();
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

        Console.WriteLine($"Top {max} projects with least VST2 plugins");
        foreach (var projectVST2Info in topProjectsWithLeastVst2Plugings)
        {
            Console.WriteLine($"Project Name: {projectVST2Info.ProjectName}");
            Console.WriteLine($"Project Path: {projectVST2Info.ProjectPath}");
            Console.WriteLine($"Distinct VST2 Plugin Count: {projectVST2Info.DistinctPluginCount}");
            Console.WriteLine($"VST2 Plugin Names: {string.Join(", ", projectVST2Info.PluginNames)}");
            Console.WriteLine();
        }
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

    private async Task RunLocate(LocateOptions options)
    {
        logger.LogDebug("Locating projects...");

        var projects = await liveProjectService.GetProjectsContainingPluginsAsync(options.PluginNamesToLocate);
        await PrintProjectsAndPlugins(projects);

        logger.LogDebug(@"Total of projects: {@TotalOfProjects}", projects.Count);
    }

    private async Task PrintProjectsAndPlugins(IEnumerable<LiveProject> projects)
    {
        logger.LogDebug("Printing projects and their details...");

        var fullJsonData = JsonSerializer.Serialize<IEnumerable<LiveProject>>(projects, new JsonSerializerOptions { WriteIndented = true });
        await Console.Out.WriteLineAsync(fullJsonData);
    }
}

internal class PluginUsageInfo
{
    public string PluginName { get; set; }
    public int UsageCount { get; set; }
    public List<string> ProjectNames { get; set; }
}