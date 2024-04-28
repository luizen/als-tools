using AlsTools.Core.Entities;
using AlsTools.Core.Enums;
using AlsTools.Core.Interfaces;
using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.ResultSets;

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
        var projects = LoadProjectsFromSetFiles(filePaths);
        await repository.InsertAsync(projects);

        return projects.Count;
    }

    public async Task<int> InitializeDbFromFoldersAsync(IEnumerable<string> folderPaths, bool includeBackupFolder)
    {
        await repository.DeleteAllAsync();
        var projects = LoadProjectsFromDirectories(folderPaths, includeBackupFolder);
        await repository.InsertAsync(projects);
        return projects.Count;
    }


    public async Task<IReadOnlyList<PluginDevice>> GetPluginUsageResults(IList<PluginDevice> availableInstalledPlugins, PluginUsageSelection selection)
    {
        // var pluginDevicePathEqualityComparer = new PluginDevicePathEqualityComparer();
        var pluginDeviceExactMatchEqualityComparer = new PluginDeviceEqualityComparer(true);
        var pluginDeviceFuzzyMatchEqualityComparer = new PluginDeviceEqualityComparer(false);
        var pluginNameEqualityComparer = new PluginDeviceNameFuzzyEqualityComparer();

        var projects = await GetAllProjectsAsync();
        var pluginsBeingUsed = projects
            .Where(proj =>
                proj.Tracks.Any(track =>
                    track.Plugins.Any())
            )
            .SelectMany(proj => 
                proj.Tracks.SelectMany(track => track.Plugins),
                (proj, plugin) => plugin with { }
            )
            .Distinct(pluginDeviceExactMatchEqualityComparer)
            .ToList();

        // var namesOfPluginsBeingUsed = projects
        //     .Where(proj =>
        //         proj.Tracks.Any(track =>
        //             track.Plugins.Any())
        //     )
        //     .SelectMany(proj =>
        //         proj.Tracks.SelectMany(track => track.Plugins),
        //         (_, plugin) => plugin.Name
        //     )
        //     .Distinct()
        //     .ToList();

        // if (selection == PluginUsageSelection.UsedOnly)
        //     return namesOfPluginsBeingUsed;

        var pluginsNotBeingUsed = new List<PluginDevice>();

        // Only plugins not being used
        foreach (var availablePlugin in availableInstalledPlugins)
        {
            if (!pluginsBeingUsed.Any(plugin => pluginDeviceFuzzyMatchEqualityComparer.Equals(plugin, availablePlugin)))
                // if (!namesOfPluginsBeingUsed.Any(pluginName => pluginNameEqualityComparer.Equals(pluginName, availablePlugin.Name)))
                pluginsNotBeingUsed.Add(availablePlugin with { });
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
        var files = fs.GetProjectFilesFullPathFromSetFiles(filePaths);

        return ExtractProjectsFromFiles(files);
    }

    private IReadOnlyList<LiveProject> LoadProjectsFromDirectories(IEnumerable<string> folderPaths, bool includeBackupFolder)
    {
        var files = fs.GetProjectFilesFullPathFromDirectories(folderPaths, includeBackupFolder);

        return ExtractProjectsFromFiles(files);
    }

    private IReadOnlyList<LiveProject> ExtractProjectsFromFiles(IEnumerable<string> filePaths)
    {
        var projects = new List<LiveProject>();

        foreach (var filePath in filePaths)
        {
            var project = extractor.ExtractProjectFromFile(filePath);
            projects.Add(project);
        }

        return projects;
    }

    public async Task<IEnumerable<TracksCountPerProjectResult>> GetTracksCountPerProject()
    {
        return await repository.GetTracksCountPerProject();
    }

    public async Task<IEnumerable<PluginsCountPerProjectResult>> GetPluginsCountPerProject(bool ignoreDisabled)
    {
        return await repository.GetPluginsCountPerProject(ignoreDisabled);
    }

    public async Task<IEnumerable<StockDevicesCountPerProjectResult>> GetStockDevicesCountPerProject(bool ignoreDisabled)
    {
        return await repository.GetStockDevicesCountPerProject(ignoreDisabled);
    }

    public async Task<IEnumerable<TracksCountPerProjectResult>> GetProjectsWithHighestTracksCount(int limit)
    {
        return await repository.GetProjectsWithHighestTracksCount(limit);
    }

    public async Task<IEnumerable<PluginsCountPerProjectResult>> GetProjectsWithHighestPluginsCount(int limit, bool ignoreDisabled)
    {
        return await repository.GetProjectsWithHighestPluginsCount(limit, ignoreDisabled);
    }

    public async Task<IEnumerable<PluginsUsageCountResult>> GetMostUsedPlugins(int limit, bool ignoreDisabled)
    {
        return await repository.GetMostUsedPlugins(limit, ignoreDisabled);
    }

    public async Task<IEnumerable<StockDevicesUsageCountResult>> GetMostUsedStockDevices(int limit, bool ignoreDisabled)
    {
        return await repository.GetMostUsedStockDevices(limit, ignoreDisabled);
    }

    public async Task<int> GetProjectsCount()
    {
        return await repository.CountProjectsAsync();
    }
}
