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

    public async Task Run()
    {
        logger.LogDebug("Starting application...");

        var projects = await liveProjectService.GetAllProjectsAsync();

        Console.Read();
    }
}