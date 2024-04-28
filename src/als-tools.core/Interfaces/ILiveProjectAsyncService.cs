using AlsTools.Core.Entities;
using AlsTools.Core.Enums;
using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.ResultSets;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncService
{
    Task<int> InitializeDbFromFilesAsync(IEnumerable<string> filePaths);

    Task<int> InitializeDbFromFoldersAsync(IEnumerable<string> folderPaths, bool includeBackupFolder);

    Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync();

    Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate);

    Task<int> CountProjectsAsync();

    Task<IReadOnlyList<PluginDevice>> GetPluginUsageResults(IList<PluginDevice> availablePlugins, PluginUsageSelection selection);

    Task<IEnumerable<TracksCountPerProjectResult>> GetTracksCountPerProject();

    Task<IEnumerable<PluginsCountPerProjectResult>> GetPluginsCountPerProject(bool ignoreDisabled);

    Task<IEnumerable<StockDevicesCountPerProjectResult>> GetStockDevicesCountPerProject(bool ignoreDisabled);

    Task<IEnumerable<TracksCountPerProjectResult>> GetProjectsWithHighestTracksCount(int limit);

    Task<IEnumerable<PluginsCountPerProjectResult>> GetProjectsWithHighestPluginsCount(int limit, bool ignoreDisabled);

    Task<IEnumerable<PluginsUsageCountResult>> GetMostUsedPlugins(int limit, bool ignoreDisabled);

    Task<IEnumerable<StockDevicesUsageCountResult>> GetMostUsedStockDevices(int limit, bool ignoreDisabled);

    Task<int> GetProjectsCount();
}
