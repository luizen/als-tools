namespace AlsTools.Ui.Cli.OptionCommandHandlers.Handlers;

public class CountCommandHandler : BaseCommandHandler, IOptionCommandHandler<CountOptions>
{
    private readonly ILogger<CountCommandHandler> logger;
    private readonly ILiveProjectAsyncService liveProjectService;

    public CountCommandHandler(ILogger<CountCommandHandler> logger, ILiveProjectAsyncService liveProjectService, IOptions<ParameterValuesOptions> parameterValuesOptions) : base(parameterValuesOptions)
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
