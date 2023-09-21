
namespace AlsTools.Ui.Cli.OptionCommandHandlers.Handlers;

public class InitDbCommandHandler : BaseCommandHandler, IOptionCommandHandler<InitDbOptions>
{
    private readonly ILogger<InitDbCommandHandler> logger;
    private readonly ILiveProjectAsyncService liveProjectService;

    public InitDbCommandHandler(ILogger<InitDbCommandHandler> logger, ILiveProjectAsyncService liveProjectService, IOptions<ParameterValuesOptions> parameterValuesOptions) : base(parameterValuesOptions)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
    }

    public async Task Execute(InitDbOptions options)
    {
        logger.LogDebug("Initializing database...");

        int count = 0;
        if (options.Files.Any())
            count = await liveProjectService.InitializeDbFromFilesAsync(options.Files);
        else
            count = await liveProjectService.InitializeDbFromFoldersAsync(options.Folders, options.IncludeBackups);

        logger.LogInformation("Total of projects loaded into DB: {@ProjectsLoadedIntoDb}", count);
    }
}
