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

    public async Task<int> InitializeDbFromFilesAsync(IEnumerable<string> filePaths, IProgress<double>? progress = null)
    {
        await repository.DeleteAllAsync();
        var projects = LoadProjectsFromSetFiles(filePaths, progress);
        await repository.InsertAsync(projects);

        return projects.Count;
    }

    public async Task<int> InitializeDbFromFoldersAsync(IEnumerable<string> folderPaths, bool includeBackupFolder, IProgress<double>? progress = null)
    {
        await repository.DeleteAllAsync();
        var projects = LoadProjectsFromDirectories(folderPaths, includeBackupFolder, progress);
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


    private IReadOnlyList<LiveProject> LoadProjectsFromSetFiles(IEnumerable<string> filePaths, IProgress<double>? progress = null)
    {
        var files = fs.GetProjectFilesFullPathFromSetFiles(filePaths);

        return ExtractProjectsFromFiles(files, progress);
    }

    private IReadOnlyList<LiveProject> LoadProjectsFromDirectories(IEnumerable<string> folderPaths, bool includeBackupFolder, IProgress<double>? progress = null)
    {
        var files = fs.GetProjectFilesFullPathFromDirectories(folderPaths, includeBackupFolder);

        return ExtractProjectsFromFiles(files, progress);
    }

    private IReadOnlyList<LiveProject> ExtractProjectsFromFiles(IEnumerable<string> filePaths, IProgress<double>? progress = null)
    {
        var projects = new List<LiveProject>();
        int count = 0;
        int total = filePaths.Count();
        double percent = 0;

        foreach (var filePath in filePaths)
        {
            var project = extractor.ExtractProjectFromFile(filePath);
            projects.Add(project);

            percent = ++count * 100 / total;
            progress?.Report(percent);
        }

        return projects;
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetTracksCountPerProject()
    {
        return await repository.GetTracksCountPerProject();
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetPluginsCountPerProject(bool ignoreDisabled)
    {
        return await repository.GetPluginsCountPerProject(ignoreDisabled);
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetStockDevicesCountPerProject(bool ignoreDisabled)
    {
        return await repository.GetStockDevicesCountPerProject(ignoreDisabled);
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetMaxForLiveDevicesCountPerProject(bool ignoreDisabled)
    {
        return await repository.GetMaxForLiveDevicesCountPerProject(ignoreDisabled);
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestTracksCount(int limit)
    {
        return await repository.GetProjectsWithHighestTracksCount(limit);
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestPluginsCount(int limit, bool ignoreDisabled)
    {
        return await repository.GetProjectsWithHighestPluginsCount(limit, ignoreDisabled);
    }

    public async Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedPlugins(int limit, bool ignoreDisabled)
    {
        return await repository.GetMostUsedPlugins(limit, ignoreDisabled);
    }

    public async Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedStockDevices(int limit, bool ignoreDisabled)
    {
        return await repository.GetMostUsedStockDevices(limit, ignoreDisabled);
    }

    public async Task<int> GetProjectsCount()
    {
        return await repository.CountProjectsAsync();
    }

    public async Task DeleteAllProjectsAsync()
    {
        await repository.DeleteAllAsync();
    }

    public async Task<LiveProject?> GetProjectByIdAsync(string id)
    {
        return await repository.GetProjectByIdAsync(id);
    }

    public async Task<IEnumerable<PluginDevice>> GetAllPluginsFromProjects()
    {
        return await repository.GetAllPluginsFromProjects();
    }

    public async Task<IEnumerable<StockDevice>> GetAllStockDevicesFromProjects()
    {
        return await repository.GetAllStockDevicesFromProjects();
    }

    public async Task<IEnumerable<MaxForLiveDevice>> GetAllMaxForLiveDevicesFromProjects()
    {
        return await repository.GetAllMaxForLiveDevicesFromProjects();
    }


}
