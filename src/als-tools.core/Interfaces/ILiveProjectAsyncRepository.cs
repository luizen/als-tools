using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncRepository
{
    Task InsertAsync(LiveProject project);

    Task InsertAsync(IEnumerable<LiveProject> projects);
    
    Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate);

    Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync();
    
    Task DeleteAllAsync();
    
    Task<int> CountProjectsAsync();

    Task<IEnumerable<TracksCountPerProjectResult>> GetTracksCountPerProject();

    Task<IEnumerable<PluginsCountPerProjectResult>> GetPluginsCountPerProject(bool ignoreDisabled);

    Task<IEnumerable<StockDevicesCountPerProjectResult>> GetStockDevicesCountPerProject(bool ignoreDisabled);

    Task<IEnumerable<TracksCountPerProjectResult>> GetProjectsWithHighestTracksCount(int limit);

    Task<IEnumerable<PluginsCountPerProjectResult>> GetProjectsWithHighestPluginsCount(int limit, bool ignoreDisabled);

    Task<IEnumerable<PluginsUsageCountResult>> GetMostUsedPlugins(int limit, bool ignoreDisabled);

    Task<IEnumerable<StockDevicesUsageCountResult>> GetMostUsedStockDevices(int limit, bool ignoreDisabled);
}
