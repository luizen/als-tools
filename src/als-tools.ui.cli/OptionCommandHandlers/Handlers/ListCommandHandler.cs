namespace AlsTools.Ui.Cli.OptionCommandHandlers.Handlers;

public class ListCommandHandler : BaseCommandHandler, IOptionCommandHandler<ListOptions>
{
    private readonly ILogger<ListCommandHandler> logger;
    private readonly ILiveProjectAsyncService liveProjectService;
    private readonly ProjectsAndPluginsPrinter projectsAndPluginsPrinter;

    public ListCommandHandler(ILogger<ListCommandHandler> logger, ILiveProjectAsyncService liveProjectService, ProjectsAndPluginsPrinter projectsAndPluginsPrinter, IOptions<ParameterValuesOptions> parameterValuesOptions) : base(parameterValuesOptions)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
        this.projectsAndPluginsPrinter = projectsAndPluginsPrinter;
    }

    public async Task Execute(ListOptions options)
    {
        logger.LogDebug("Listing projects...");

        var projects = await liveProjectService.GetAllProjectsAsync();
        await projectsAndPluginsPrinter.Print(projects);

        logger.LogDebug(@"Total of projects: {@TotalOfProjects}", projects.Count);

    }
}
