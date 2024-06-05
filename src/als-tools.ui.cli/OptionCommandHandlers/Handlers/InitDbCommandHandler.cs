
namespace AlsTools.Ui.Cli.OptionCommandHandlers.Handlers;

public class InitDbCommandHandler : BaseCommandHandler, IOptionCommandHandler<CliOptions.InitDbOptions>
{
    private readonly ILogger<InitDbCommandHandler> logger;
    private readonly ILiveProjectAsyncService liveProjectService;

    public InitDbCommandHandler(ILogger<InitDbCommandHandler> logger, ILiveProjectAsyncService liveProjectService, IOptions<ParameterValuesOptions> parameterValuesOptions) : base(parameterValuesOptions)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
    }

    public async Task Execute(CliOptions.InitDbOptions options)
    {
        logger.LogDebug("Initializing database...");

        int count = 0;
        if (options.Files.Any())
            count = await liveProjectService.InitializeDbFromPathsAsync(options.Files);
        else
            count = await liveProjectService.InitializeDbFromPathsAsync(options.Folders, options.IncludeBackups);

        logger.LogInformation("Total of projects loaded into DB: {@ProjectsLoadedIntoDb}", count);
    }
}
