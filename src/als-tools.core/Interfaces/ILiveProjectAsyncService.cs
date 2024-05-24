using AlsTools.Core.Entities;
using AlsTools.Core.Enums;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.ResultSets;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncService
{
    Task<int> InitializeDbFromFilesAsync(IEnumerable<string> filePaths, IProgress<double>? progress = null);

    Task<int> InitializeDbFromFoldersAsync(IEnumerable<string> folderPaths, bool includeBackupFolder, IProgress<double>? progress = null);

    Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync();

    Task<LiveProject?> GetProjectByIdAsync(string id);

    Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate);

    Task<int> CountProjectsAsync();

    Task<IReadOnlyList<PluginDevice>> GetPluginUsageResults(IList<PluginDevice> availablePlugins, PluginUsageSelection selection);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetTracksCountPerProject();

    Task<IEnumerable<ItemsCountPerProjectResult>> GetPluginsCountPerProject(bool ignoreDisabled);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetStockDevicesCountPerProject(bool ignoreDisabled);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetMaxForLiveDevicesCountPerProject(bool ignoreDisabled);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestTracksCount(int limit);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestPluginsCount(int limit, bool ignoreDisabled);

    Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedPlugins(int limit, bool ignoreDisabled);

    Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedStockDevices(int limit, bool ignoreDisabled);

    Task<int> GetProjectsCount();

    Task DeleteAllProjectsAsync();

    Task<IEnumerable<PluginDevice>> GetAllPluginsFromProjects();

    Task<IEnumerable<StockDevice>> GetAllStockDevicesFromProjects();

    Task<IEnumerable<MaxForLiveDevice>> GetAllMaxForLiveDevicesFromProjects();
}
