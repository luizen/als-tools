using AlsTools.Core.Entities;
using AlsTools.Core.Enums;
using AlsTools.Core.Interfaces;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.Services;

public class LiveProjectAsyncService : ILiveProjectAsyncService
{
    private readonly ILogger<LiveProjectAsyncService> logger;
    private readonly ILiveProjectAsyncRepository repository;
    private readonly ILiveProjectFileSystem fs;
    private readonly ILiveProjectFileExtractionHandler extractor;

    public LiveProjectAsyncService(ILogger<LiveProjectAsyncService> logger, ILiveProjectAsyncRepository repository, ILiveProjectFileSystem fs, ILiveProjectFileExtractionHandler extractor)
    {
        this.logger = logger;
        this.repository = repository;
        this.fs = fs;
        this.extractor = extractor;
    }

    public async Task<int> CountProjectsAsync()
    {
        return await repository.CountProjectsAsync();
    }

    public async Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync()
    {
        return await repository.GetAllProjectsAsync();
    }

    public async Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate)
    {
        return await repository.GetProjectsContainingPluginsAsync(pluginsToLocate);
    }

    public async Task<int> InitializeDbFromFilesAsync(IEnumerable<string> filePaths)
    {
        await repository.DeleteAllAsync();
        var project = LoadProjectsFromSetFiles(filePaths);
        await repository.InsertAsync(project);

        return 1;
    }

    public async Task<int> InitializeDbFromFoldersAsync(IEnumerable<string> folderPaths, bool includeBackupFolder)
    {
        await repository.DeleteAllAsync();
        var projects = LoadProjectsFromDirectories(folderPaths, includeBackupFolder);
        await repository.InsertAsync(projects);
        return projects.Count;
    }


    public async Task<IReadOnlyList<PluginDevice>> GetPluginUsageResults(IList<PluginDevice> availablePlugins, PluginUsageSelection selection)
    {
        // var pluginDevicePathEqualityComparer = new PluginDevicePathEqualityComparer();
        var pluginDeviceEqualityComparer = new PluginDeviceEqualityComparer();

        var projects = await GetAllProjectsAsync();
        var pluginsBeingUsed = projects
            .Where(proj =>
                // proj.Tracks != null && 
                // proj.Tracks.Any() && 
                proj.Tracks.Any(track =>
                    // track.Plugins != null && 
                    track.Plugins.Any())
            )
            .SelectMany(proj => 
                proj.Tracks.SelectMany(track => track.Plugins),
                (proj, plugin) => (PluginDevice)plugin.Clone()
            )
            //.Distinct(pluginDevicePathEqualityComparer)
            .Distinct(pluginDeviceEqualityComparer)
            .ToList();

        if (selection == PluginUsageSelection.UsedOnly)
            return pluginsBeingUsed;

        var pluginsNotBeingUsed = new List<PluginDevice>();

        // Only plugins not being used
        foreach (var availablePlugin in availablePlugins)
        {
            // if (!pluginsBeingUsed.Any(plugin => pluginDevicePathEqualityComparer.Equals(plugin, availablePlugin)))
            if (!pluginsBeingUsed.Any(plugin => pluginDeviceEqualityComparer.Equals(plugin, availablePlugin)))
                pluginsNotBeingUsed.Add((PluginDevice)availablePlugin.Clone());
        }

        return pluginsNotBeingUsed;
    }


// VST2 = LittlePlate, lots of details in the project file
// VST3 = Little Plate, very few details in the project file
// In both plugin Info.plist files there is only LittlePlate.
// Where the hell does Ableton get the plugin name from?

// USE FUZZY SEARCH!


    private IReadOnlyList<LiveProject> LoadProjectsFromSetFiles(IEnumerable<string> filePaths)
    {
        var files = fs.LoadProjectFilesFromSetFiles(filePaths);

        return ExtractProjectsFromFiles(files);
    }

    private IReadOnlyList<LiveProject> LoadProjectsFromDirectories(IEnumerable<string> folderPaths, bool includeBackupFolder)
    {
        var files = fs.LoadProjectFilesFromDirectories(folderPaths, includeBackupFolder);

        return ExtractProjectsFromFiles(files);
    }

    private IReadOnlyList<LiveProject> ExtractProjectsFromFiles(IEnumerable<FileInfo> files)
    {
        var projects = new List<LiveProject>();

        foreach (var f in files)
        {
            var project = extractor.ExtractProjectFromFile(f);
            projects.Add(project);
        }

        return projects;
    }
}
