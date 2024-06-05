namespace AlsTools.Ui.Cli;

public class App
{
    private readonly ILogger<App> logger;
    private readonly ILiveProjectAsyncService liveProjectService;
    private readonly IOptions<PlugInfoOptions> plugInfoOptions;
    private readonly IOptionCommandHandler<CliOptions.InitDbOptions> initDbCommandHandler;
    private readonly IOptionCommandHandler<ListOptions> listCommandHandler;
    private readonly IOptionCommandHandler<CountOptions> countCommandHandler;
    private readonly IOptionCommandHandler<LocateOptions> locateCommandHandler;
    private readonly IOptionCommandHandler<PluginUsageOptions> pluginUsageCommandHandler;
    private readonly IOptionCommandHandler<PrintStatisticsOptions> printStatisticsCommandHandler;

    // private readonly IOptions<PlugScanningOptions> plugScanningOptions;

    public App(
        ILogger<App> logger,
        ILiveProjectAsyncService liveProjectService,
        IOptions<PlugInfoOptions> plugInfoOptions,
        IOptionCommandHandler<CliOptions.InitDbOptions> initDbCommandHandler,
        IOptionCommandHandler<ListOptions> listCommandHandler,
        IOptionCommandHandler<CountOptions> countCommandHandler,
        IOptionCommandHandler<LocateOptions> locateCommandHandler,
        IOptionCommandHandler<PluginUsageOptions> pluginUsageCommandHandler,
        IOptionCommandHandler<PrintStatisticsOptions> printStatisticsCommandHandler)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
        this.plugInfoOptions = plugInfoOptions;
        this.initDbCommandHandler = initDbCommandHandler;
        this.listCommandHandler = listCommandHandler;
        this.countCommandHandler = countCommandHandler;
        this.locateCommandHandler = locateCommandHandler;
        this.pluginUsageCommandHandler = pluginUsageCommandHandler;
        this.printStatisticsCommandHandler = printStatisticsCommandHandler;
        // this.plugScanningOptions = plugScanningOptions;
    }

    public async Task Run(ParserResult<object> parserResult)
    {
        logger.LogDebug("Starting application...");

        await parserResult.WithParsedAsync<CliOptions.InitDbOptions>(options => initDbCommandHandler.Execute(options));
        await parserResult.WithParsedAsync<CountOptions>(options => countCommandHandler.Execute(options));
        await parserResult.WithParsedAsync<ListOptions>(options => listCommandHandler.Execute(options));
        await parserResult.WithParsedAsync<PrintStatisticsOptions>(options => printStatisticsCommandHandler.Execute(options));
        await parserResult.WithParsedAsync<PluginUsageOptions>(options => pluginUsageCommandHandler.Execute(options));
        await parserResult.WithParsedAsync<LocateOptions>(options => locateCommandHandler.Execute(options));
        await parserResult.WithNotParsedAsync(errors => { throw new CommandLineParseException(errors); });
    }
}