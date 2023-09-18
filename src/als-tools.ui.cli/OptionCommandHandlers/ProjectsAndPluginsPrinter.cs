namespace AlsTools.Ui.Cli;

public class ProjectsAndPluginsPrinter
{
    private readonly ILogger<ProjectsAndPluginsPrinter> logger;

    public ProjectsAndPluginsPrinter(ILogger<ProjectsAndPluginsPrinter> logger)
    {
        this.logger = logger;
    }

    public async Task Print(IEnumerable<LiveProject> projects)
    {
        logger.LogDebug("Printing projects and their details...");

        var fullJsonData = JsonSerializer.Serialize(projects, new JsonSerializerOptions { WriteIndented = true });
        await Console.Out.WriteLineAsync(fullJsonData);
    }
}
