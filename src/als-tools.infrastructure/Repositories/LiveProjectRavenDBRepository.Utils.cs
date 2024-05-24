using AlsTools.Core.Interfaces;
using AlsTools.Core.ValueObjects.ResultSets;
using AlsTools.Infrastructure.Indexes;
using Raven.Client.Documents.Linq;

namespace AlsTools.Infrastructure.Repositories;

public partial class LiveProjectRavenRepository : ILiveProjectAsyncRepository
{
    private IRavenQueryable<ItemsCountPerProjectResult> GetPluginsCountDisabledQuery(bool ignoreDisabled)
    {
        return ignoreDisabled
            ? store.OpenAsyncSession().Query<ItemsCountPerProjectResult, LiveProjects_PluginsCount>().Where(result => result.IsEnabled)
            : store.OpenAsyncSession().Query<ItemsCountPerProjectResult, LiveProjects_PluginsCount>();
    }

    private IRavenQueryable<ItemsCountPerProjectResult> GetStockDevicesCountDisabledQuery(bool ignoreDisabled)
    {
        return ignoreDisabled
            ? store.OpenAsyncSession().Query<ItemsCountPerProjectResult, LiveProjects_StockDevicesCount_EnabledOnly>()
            : store.OpenAsyncSession().Query<ItemsCountPerProjectResult, LiveProjects_StockDevicesCount>();
    }

    private IRavenQueryable<ItemsCountPerProjectResult> GetMaxForLiveDevicesCountDisabledQuery(bool ignoreDisabled)
    {
        return ignoreDisabled
            ? store.OpenAsyncSession().Query<ItemsCountPerProjectResult, LiveProjects_MaxForLiveDevicesCount>().Where(result => result.IsEnabled)
            : store.OpenAsyncSession().Query<ItemsCountPerProjectResult, LiveProjects_MaxForLiveDevicesCount>();
    }
}
