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

    Task<IEnumerable<LiveProject>> GetAllProjectsAsync();

    Task<LiveProject?> GetProjectByIdAsync(string id);

    Task<IEnumerable<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate);

    Task<int> CountProjectsAsync();

    Task<IEnumerable<PluginDevice>> GetPluginUsageResults(IList<PluginDevice> availablePlugins, PluginUsageSelection selection);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetTracksCountPerProject();

    Task<IEnumerable<ItemsCountPerProjectResult>> GetPluginsCountPerProject(bool ignoreDisabled = false);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetStockDevicesCountPerProject(bool ignoreDisabled = false);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetMaxForLiveDevicesCountPerProject(bool ignoreDisabled = false);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestTracksCount(int? limit = null);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestPluginsCount(int? limit = null, bool ignoreDisabled = false);

    Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedPlugins(int? limit = null, bool ignoreDisabled = false);

    Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedStockDevices(int? limit = null, bool ignoreDisabled = false);

    Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedMaxForLiveDevices(int? limit = null, bool ignoreDisabled = false);

    Task<int> GetProjectsCount();

    Task DeleteAllProjectsAsync();

    Task<IEnumerable<PluginDevice>> GetAllPluginsFromProjects(int? limit = null, bool ignoreDisabled = false);

    Task<IEnumerable<StockDevice>> GetAllStockDevicesFromProjects(int? limit = null, bool ignoreDisabled = false);

    Task<IEnumerable<MaxForLiveDevice>> GetAllMaxForLiveDevicesFromProjects(int? limit = null, bool ignoreDisabled = false);
}
