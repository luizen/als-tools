using AlsTools.Core.Interfaces;
using AlsTools.Core.ValueObjects.ResultSets;
using AlsTools.Infrastructure.Indexes;
using Raven.Client.Documents.Linq;

namespace AlsTools.Infrastructure.Repositories;

public partial class LiveProjectRavenRepository : ILiveProjectAsyncRepository
{
    private IRavenQueryable<PluginsCountPerProjectResult> GetPluginsCountDisabledQuery(bool ignoreDisabled)
    {
        return ignoreDisabled
            ? store.OpenAsyncSession().Query<PluginsCountPerProjectResult, LiveProjects_PluginsCount_EnabledOnly>()
            : store.OpenAsyncSession().Query<PluginsCountPerProjectResult, LiveProjects_PluginsCount>();
    }

    private IRavenQueryable<StockDevicesCountPerProjectResult> GetStockDevicesCountDisabledQuery(bool ignoreDisabled)
    {
        return ignoreDisabled
            ? store.OpenAsyncSession().Query<StockDevicesCountPerProjectResult, LiveProjects_StockDevicesCount_EnabledOnly>()
            : store.OpenAsyncSession().Query<StockDevicesCountPerProjectResult, LiveProjects_StockDevicesCount>();
    }
}
