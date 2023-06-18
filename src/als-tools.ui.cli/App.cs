namespace AlsTools.Ui.Cli;

public class App
{
    private readonly ILogger<App> logger;
    private readonly ILiveProjectAsyncService liveProjectService;

    public App(ILogger<App> logger, ILiveProjectAsyncService liveProjectService)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
    }

    public async Task Run(ParserResult<object> parserResult)
    {
        logger.LogDebug("Starting application...");

        await parserResult.WithParsedAsync<InitDbOptions>(options => RunInitDb(options));
        await parserResult.WithParsedAsync<CountOptions>(options => RunCount(options));
        await parserResult.WithParsedAsync<ListOptions>(options => RunList(options));
        await parserResult.WithParsedAsync<LocateOptions>(options => RunLocate(options));
        await parserResult.WithNotParsedAsync(errors => { throw new CommandLineParseException(errors); });
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
        logger.LogDebug("Listing projects or plugin names...");

        var projects = await liveProjectService.GetAllProjectsAsync();

        if (options.PluginNamesOnly)
            await PrintPluginNames(projects);
        else
            await PrintProjectsAndPlugins(projects, false);
    }

    private async Task RunLocate(LocateOptions options)
    {
        logger.LogDebug("Locating projects...");

        var projects = await liveProjectService.Search(options.MapToSpecification());
        await PrintProjectsAndPlugins(projects, options.CompactOutput);

        logger.LogDebug(@"Total of projects: {@TotalOfProjects}", projects.Count);
    }

    private async Task PrintProjectsAndPlugins(IEnumerable<LiveProject> projects, bool compactOutput)
    {
        logger.LogDebug("Printing projects and their details. CompactOutput: {@CompactOutput}", compactOutput);

        if (compactOutput)
        {
            var compactItems = projects.Select(p => new
            {
                ProjectName = p.Name,
                ProjectPath = p.Path
            });

            var compactJsonData = JsonSerializer.Serialize<IEnumerable<dynamic>>(compactItems, new JsonSerializerOptions { WriteIndented = true });
            await Console.Out.WriteLineAsync(compactJsonData);
        }
        else
        {
            var fullJsonData = JsonSerializer.Serialize<IEnumerable<LiveProject>>(projects, new JsonSerializerOptions { WriteIndented = true });
            await Console.Out.WriteLineAsync(fullJsonData);
        }

        logger.LogDebug(@"Total of projects: {@TotalOfProjects}", projects.Count());
    }

    private async Task PrintPluginNames(IEnumerable<LiveProject> projects)
    {
        logger.LogDebug("Printing plugin names...");

        var plugins = projects
            .Where(proj => proj.Tracks != null && proj.Tracks.Any() && proj.Tracks.All(track => track.Plugins != null && track.Plugins.Any()))
            .SelectMany(proj => proj.Tracks.SelectMany(track => track.Plugins))
            .Select(plugin =>
            new
            {
                PluginName = plugin.Name,
                PluginFormat = plugin.Format.ToString()
            })
            .Distinct()
            .ToList();

        var pluginNamesWithProject = projects
            .Where(proj => proj.Tracks != null && proj.Tracks.Any() && proj.Tracks.All(track => track.Plugins != null && track.Plugins.Any()))
            .SelectMany(proj => proj.Tracks.SelectMany(track => track.Plugins), (proj, plugin) => new
            {
                ProjectName = proj.Name,
                PluginName = plugin.Name,
                PluginFormat = plugin.Format.ToString()
            })
            .Distinct()
            .ToList();

        // var tracks = projects.Where(proj => proj.Tracks.Any(track => track.Plugins.Any())).Select(proj => proj.);

        // var pluginNames = projects.Select(project => project.Tracks.Select(track => track.Plugins.Select(plugin => new 
        // {
        //     PluginName = plugin.Name,
        //     PluginFormat = plugin.Format
        // }))).Where()

        //()Distinct().ToList();

        var json = JsonSerializer.Serialize<IEnumerable<dynamic>>(pluginNamesWithProject, new JsonSerializerOptions { WriteIndented = true });
        await Console.Out.WriteLineAsync(json);

        logger.LogDebug(@"Total of distinct plugins: {@TotalOfPlugins}", plugins.Count);
    }
}
