
namespace AlsTools.Ui.Cli;

public class CountCommandHandler : IOptionCommandHandler<CountOptions>
{
    private readonly ILogger<CountCommandHandler> logger;
    private readonly ILiveProjectAsyncService liveProjectService;

    public CountCommandHandler(ILogger<CountCommandHandler> logger, ILiveProjectAsyncService liveProjectService)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
    }

    public async Task Execute(CountOptions options)
    {
        logger.LogDebug("Counting projects...");

        int count = await liveProjectService.CountProjectsAsync();

        await Console.Out.WriteLineAsync($"\nTotal of projects in the DB: {count}");
    }
}
