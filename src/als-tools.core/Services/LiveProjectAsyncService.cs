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
        try
        {
            logger.LogInformation("Counting projects...");
            return await repository.CountProjectsAsync();
        }
        finally
        {
            logger.LogInformation("Counting projects... DONE");
        }
    }

    public async Task<IEnumerable<LiveProject>> GetAllProjectsAsync(int? limit = null)
    {
        try
        {
            logger.LogInformation("Getting all projects...");
            return await repository.GetAllProjectsAsync(limit);
        }
        finally
        {
            logger.LogInformation("Getting all projects... DONE");
        }
    }

    public async Task<IEnumerable<LiveProjectWithChildrenCountsResult>> GetAllProjectsWithChildrenCountsAsync(int? limit = null)
    {
        try
        {
            logger.LogInformation("Getting all projects, including the children counts...");
            return await repository.GetAllProjectsWithChildrenCountsAsync(limit);
        }
        finally
        {
            logger.LogInformation("Getting all projects, including the children counts... DONE");
        }
    }

    public async Task<IEnumerable<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate)
    {
        try
        {
            logger.LogInformation("Getting projects containing given plugins: {@PluginsToLocate}.", pluginsToLocate);
            return await repository.GetProjectsContainingPluginsAsync(pluginsToLocate);
        }
        finally
        {
            logger.LogInformation("Getting projects containing given plugins: {@PluginsToLocate}. DONE", pluginsToLocate);
        }
    }

    public async Task<int> InitializeDbFromPathsAsync(IEnumerable<string> paths, bool includeBackupFolder, IProgress<double>? progress = null)
    {
        try
        {
            logger.LogInformation("Initializing database from paths: {@Paths}.", paths);

            await repository.DeleteAllAsync();
            var projects = LoadProjectsFromPaths(paths, includeBackupFolder, progress);
            await repository.InsertAsync(projects);

            logger.LogInformation("Database initialized with {@ProjectsCount} projects", projects.Count);

            return projects.Count;
        }
        finally
        {
            logger.LogInformation("Initializing database from paths: {@Paths}. DONE", paths);
        }
    }

    public async Task ReloadAllFileDatesAsync(IProgress<double>? progress = null)
    {
        try
        {
            logger.LogInformation("Loading latest dates for all project files...");

            var projects = await repository.GetAllProjectsAsync();
            int total = projects.Count();
            int count = 0;
            double percent = 0;

            foreach (var project in projects)
            {
                fs.SetFileDates(project);

                logger.LogDebug("Loaded and set dates for project {@ProjectName}. CreationTime: {@CreationTime}. LastModified: {@LastModified}",
                    project.Name, project.CreationTime, project.LastModified);

                await repository.UpdateAsync(project);

                percent = ++count * 100 / total;
                progress?.Report(percent);
            }
        }
        finally
        {
            logger.LogInformation("Loading latest dates for all project files... DONE");
        }
    }


    public async Task<IEnumerable<PluginDevice>> GetPluginUsageResults(IList<PluginDevice> availableInstalledPlugins, PluginUsageSelection selection)
    {
        try
        {
            logger.LogInformation("Getting plugin usage results...");

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
        finally
        {
            logger.LogInformation("Getting plugin usage results... DONE");
        }
    }


    // VST2 = LittlePlate, lots of details in the project file
    // VST3 = Little Plate, very few details in the project file
    // In both plugin Info.plist files there is only LittlePlate.
    // Where the hell does Ableton get the plugin name from?

    // USE FUZZY SEARCH!

    private IList<LiveProject> LoadProjectsFromPaths(IEnumerable<string> paths, bool includeBackupFolder = false, IProgress<double>? progress = null)
    {
        try
        {
            logger.LogInformation("Loading projects from paths: {@Paths}.", paths);

            var files = fs.GetProjectFilesFullPathFromPaths(paths, includeBackupFolder);

            return ExtractProjectsFromFiles(files, progress);
        }
        finally
        {
            logger.LogInformation("Loading projects from paths: {@Paths}. DONE", paths);
        }
    }

    private IList<LiveProject> ExtractProjectsFromFiles(IEnumerable<string> filePaths, IProgress<double>? progress = null)
    {

        try
        {
            logger.LogInformation("Extracting projects from files...");

            var projects = new List<LiveProject>();
            int count = 0;
            int total = filePaths.Count();
            double percent = 0;

            foreach (var filePath in filePaths)
            {
                var project = extractor.ExtractProjectFromFile(filePath);

                fs.SetFileDates(project);

                projects.Add(project);

                percent = ++count * 100 / total;
                progress?.Report(percent);
            }

            return projects;
        }
        finally
        {
            logger.LogInformation("Extracting projects from files... DONE");
        }
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetTracksCountPerProject()
    {
        try
        {
            logger.LogInformation("Getting tracks count per project...");
            return await repository.GetTracksCountPerProject();
        }
        finally
        {
            logger.LogInformation("Getting tracks count per project... DONE");
        }
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetPluginsCountPerProject(bool ignoreDisabled = false)
    {
        try
        {
            logger.LogInformation("Getting plugins count per project...");
            return await repository.GetPluginsCountPerProject(ignoreDisabled);
        }
        finally
        {
            logger.LogInformation("Getting plugins count per project... DONE");
        }
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetStockDevicesCountPerProject(bool ignoreDisabled = false)
    {
        try
        {
            logger.LogInformation("Getting stock devices count per project...");
            return await repository.GetStockDevicesCountPerProject(ignoreDisabled);
        }
        finally
        {
            logger.LogInformation("Getting stock devices count per project... DONE");
        }
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetMaxForLiveDevicesCountPerProject(bool ignoreDisabled = false)
    {
        try
        {
            logger.LogInformation("Getting Max for Live devices count per project...");
            return await repository.GetMaxForLiveDevicesCountPerProject(ignoreDisabled);
        }
        finally
        {
            logger.LogInformation("Getting Max for Live devices count per project... DONE");
        }
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestTracksCount(int? limit = null)
    {
        try
        {
            logger.LogInformation("Getting projects with highest tracks count...");
            return await repository.GetProjectsWithHighestTracksCount(limit);
        }
        finally
        {
            logger.LogInformation("Getting projects with highest tracks count... DONE");
        }
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestPluginsCount(int? limit = null, bool ignoreDisabled = false)
    {
        try
        {
            logger.LogInformation("Getting projects with highest plugins count...");
            return await repository.GetProjectsWithHighestPluginsCount(limit, ignoreDisabled);
        }
        finally
        {
            logger.LogInformation("Getting projects with highest plugins count... DONE");
        }
    }

    public async Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedPlugins(int? limit = null, bool ignoreDisabled = false)
    {
        try
        {
            logger.LogInformation("Getting most used plugins...");
            return await repository.GetMostUsedPlugins(limit, ignoreDisabled);
        }
        finally
        {
            logger.LogInformation("Getting most used plugins... DONE");
        }
    }

    public async Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedStockDevices(int? limit = null, bool ignoreDisabled = false)
    {
        try
        {
            logger.LogInformation("Getting most used stock devices...");
            return await repository.GetMostUsedStockDevices(limit, ignoreDisabled);
        }
        finally
        {
            logger.LogInformation("Getting most used stock devices... DONE");
        }
    }

    public async Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedMaxForLiveDevices(int? limit = null, bool ignoreDisabled = false)
    {
        try
        {
            logger.LogInformation("Getting most used Max for Live devices...");
            return await repository.GetMostUsedMaxForLiveDevices(limit, ignoreDisabled);
        }
        finally
        {
            logger.LogInformation("Getting most used Max for Live devices... DONE");
        }
    }

    public async Task DeleteAllProjectsAsync()
    {
        try
        {
            logger.LogInformation("Deleting all projects...");
            await repository.DeleteAllAsync();
        }
        finally
        {
            logger.LogInformation("Deleting all projects... DONE");
        }
    }

    public async Task<LiveProject?> GetProjectByIdAsync(string id)
    {
        try
        {
            logger.LogInformation("Getting project by id: {@ProjectId}.", id);
            return await repository.GetProjectByIdAsync(id);
        }
        finally
        {
            logger.LogInformation("Getting project by id: {@ProjectId}. DONE", id);
        }
    }

    public async Task<IEnumerable<PluginDevice>> GetAllPluginsFromProjects(int? limit = null, bool ignoreDisabled = false)
    {
        try
        {
            logger.LogInformation("Getting all plugins from all projects...");
            return await repository.GetAllPluginsFromProjects(limit, ignoreDisabled);
        }
        finally
        {
            logger.LogInformation("Getting all plugins from all projects... DONE");
        }
    }

    public async Task<IEnumerable<StockDevice>> GetAllStockDevicesFromProjects(int? limit = null, bool ignoreDisabled = false)
    {
        try
        {
            logger.LogInformation("Getting all stock devices from all projects...");
            return await repository.GetAllStockDevicesFromProjects(limit, ignoreDisabled);
        }
        finally
        {
            logger.LogInformation("Getting all stock devices from all projects... DONE");
        }
    }

    public async Task<IEnumerable<MaxForLiveDevice>> GetAllMaxForLiveDevicesFromProjects(int? limit = null, bool ignoreDisabled = false)
    {
        try
        {
            logger.LogInformation("Getting all Max for Live devices from all projects...");
            return await repository.GetAllMaxForLiveDevicesFromProjects(limit, ignoreDisabled);
        }
        finally
        {
            logger.LogInformation("Getting all Max for Live devices from all projects... DONE");
        }

    }
}
