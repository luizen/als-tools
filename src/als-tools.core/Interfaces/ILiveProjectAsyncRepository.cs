using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncRepository
{
    Task InsertAsync(LiveProject project);

    Task InsertAsync(IEnumerable<LiveProject> projects);
    
    Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate);

    Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync();

    Task<LiveProject?> GetProjectByIdAsync(string id);

    Task DeleteAllAsync();
    
    Task<int> CountProjectsAsync();

    Task<IEnumerable<ItemsCountPerProjectResult>> GetTracksCountPerProject();

    Task<IEnumerable<ItemsCountPerProjectResult>> GetPluginsCountPerProject(bool ignoreDisabled);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetStockDevicesCountPerProject(bool ignoreDisabled);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestTracksCount(int limit);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestPluginsCount(int limit, bool ignoreDisabled);

    Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedPlugins(int limit, bool ignoreDisabled);

    Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedStockDevices(int limit, bool ignoreDisabled);


}
