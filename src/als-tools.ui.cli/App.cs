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

        logger.LogDebug(@"Total of projects: {@TotalOfProjects}", projects.Count);
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
