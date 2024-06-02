using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.ResultSets;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncRepository
{
    Task InsertAsync(LiveProject project);

    Task InsertAsync(IEnumerable<LiveProject> projects);

    Task<IEnumerable<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate);

    Task<IEnumerable<LiveProject>> GetAllProjectsAsync(int? limit = null);

    Task<LiveProject?> GetProjectByIdAsync(string id);

    Task DeleteAllAsync();
    
    Task<int> CountProjectsAsync();

    Task<IEnumerable<ItemsCountPerProjectResult>> GetTracksCountPerProject();

    Task<IEnumerable<ItemsCountPerProjectResult>> GetPluginsCountPerProject(bool ignoreDisabled = false);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetStockDevicesCountPerProject(bool ignoreDisabled = false);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetMaxForLiveDevicesCountPerProject(bool ignoreDisabled = false);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestTracksCount(int? limit = null);

    Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestPluginsCount(int? limit = null, bool ignoreDisabled = false);

    Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedPlugins(int? limit = null, bool ignoreDisabled = false);

    Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedStockDevices(int? limit, bool ignoreDisabled = false);

    Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedMaxForLiveDevices(int? limit, bool ignoreDisabled);

    Task<IEnumerable<PluginDevice>> GetAllPluginsFromProjects(int? limit = null, bool ignoreDisabled = false);

    Task<IEnumerable<StockDevice>> GetAllStockDevicesFromProjects(int? limit = null, bool ignoreDisabled = false);

    Task<IEnumerable<MaxForLiveDevice>> GetAllMaxForLiveDevicesFromProjects(int? limit = null, bool ignoreDisabled = false);


}
