using AlsTools.CliOptions;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using AlsTools.Exceptions;
using CommandLine;

namespace AlsTools;

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
        logger.LogDebug("Listing projects...");

        var projects = await liveProjectService.GetAllProjectsAsync();
        await PrintProjectsAndPlugins(projects);

        logger.LogDebug(@"Total of projects: {@TotalOfProjects}", projects.Count);
    }

    private async Task RunLocate(LocateOptions options)
    {
        logger.LogDebug("Locating projects...");

        var projects = await liveProjectService.GetProjectsContainingPluginsAsync(options.PluginsToLocate);
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
