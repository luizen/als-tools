namespace AlsTools.Ui.Cli.OptionCommandHandlers.Handlers;

public class LocateCommandHandler : BaseCommandHandler, IOptionCommandHandler<LocateOptions>
{
    private readonly ILogger<LocateCommandHandler> logger;
    private readonly ILiveProjectAsyncService liveProjectService;
    private readonly ProjectsAndPluginsPrinter projectsAndPluginsPrinter;

    public LocateCommandHandler(ILogger<LocateCommandHandler> logger, ILiveProjectAsyncService liveProjectService, ProjectsAndPluginsPrinter projectsAndPluginsPrinter, IOptions<ParameterValuesOptions> parameterValuesOptions) : base(parameterValuesOptions)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
        this.projectsAndPluginsPrinter = projectsAndPluginsPrinter;
    }

    public async Task Execute(LocateOptions options)
    {
        logger.LogDebug("Locating projects...");

        var projects = await liveProjectService.GetProjectsContainingPluginsAsync(options.PluginNamesToLocate);
        await projectsAndPluginsPrinter.Print(projects);

        logger.LogDebug(@"Total of projects: {@TotalOfProjects}", projects.Count);
    }
}
