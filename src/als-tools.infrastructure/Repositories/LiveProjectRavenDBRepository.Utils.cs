using AlsTools.Core.Interfaces;
using Raven.Client.Documents.Linq;

namespace AlsTools.Infrastructure.Repositories;

public partial class LiveProjectRavenRepository : ILiveProjectAsyncRepository
{
    private IRavenQueryable<ItemsCountPerProjectResult> GetPluginsCountDisabledQuery(bool ignoreDisabled)
    {
        return ignoreDisabled
            ? store.OpenAsyncSession().Query<ItemsCountPerProjectResult, LiveProjects_PluginsCount_EnabledOnly>()
            : store.OpenAsyncSession().Query<ItemsCountPerProjectResult, LiveProjects_PluginsCount>();
    }

    private IRavenQueryable<ItemsCountPerProjectResult> GetStockDevicesCountDisabledQuery(bool ignoreDisabled)
    {
        return ignoreDisabled
            ? store.OpenAsyncSession().Query<ItemsCountPerProjectResult, LiveProjects_StockDevicesCount_EnabledOnly>()
            : store.OpenAsyncSession().Query<ItemsCountPerProjectResult, LiveProjects_StockDevicesCount>();
    }
}
