using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.ResultSets;
using AlsTools.Infrastructure.Indexes;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Operations;

namespace AlsTools.Infrastructure.Repositories;

public partial class LiveProjectRavenRepository : ILiveProjectAsyncRepository
{
    private readonly ILogger<LiveProjectRavenRepository> logger;
    private readonly IEmbeddedDatabaseContext dbContext;
    private readonly IDocumentStore store;
    private readonly string collectionName;

    public LiveProjectRavenRepository(ILogger<LiveProjectRavenRepository> logger, IEmbeddedDatabaseContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.store = dbContext.DocumentStore;
        this.collectionName = store.Conventions.GetCollectionName(typeof(LiveProject));
    }

    public async Task InsertAsync(LiveProject project)
    {
        using (var session = store.OpenAsyncSession())
        {
            await session.StoreAsync(project);
            await session.SaveChangesAsync();
        }

        logger.LogDebug("Inserted project {ProjectName}", project.Name);
    }

    public async Task InsertAsync(IEnumerable<LiveProject> projects)
    {
        await using var bulkInsert = store.BulkInsert();
        int count = 0;
        foreach (var project in projects)
        {
            await bulkInsert.StoreAsync(project);
            count++;
        }

        logger.LogDebug("Inserted {InsertedProjects} projects", count);
    }

    public async Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate)
    {
        using var session = store.OpenAsyncSession();
        var results = await session
            .Query<LiveProject, LiveProjects_ByPluginNames>()
            .Where(plugin => plugin.Name.In(pluginsToLocate))
            .ToListAsync();

        return results;
    }

    public async Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync(int? limit = null)
    {
        using var session = store.OpenAsyncSession();
        return await session.Query<LiveProject>()
            .OrderBy(p => p.Name)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<LiveProject?> GetProjectByIdAsync(string id)
    {
        using var session = store.OpenAsyncSession();
        return await session.LoadAsync<LiveProject>(id);
    }

    public async Task DeleteAllAsync()
    {
        var operation = await store
            .Operations
            .SendAsync(new DeleteByQueryOperation($"from {collectionName}"));

        operation.WaitForCompletion(TimeSpan.FromSeconds(5));
    }

    public async Task<int> CountProjectsAsync()
    {
        using var session = store.OpenAsyncSession();
        return await session.Query<LiveProject>().CountAsync();
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetTracksCountPerProject()
    {
        using var session = store.OpenAsyncSession();
        return await session.Query<ItemsCountPerProjectResult, LiveProjects_TracksCount>()
            .OrderByDescending(result => result.ItemsCount)
            .ToListAsync();
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetPluginsCountPerProject(bool ignoreDisabled = false)
    {
        using var session = store.OpenAsyncSession();
        var query = session.GetIgnoreDisabledQuery<ItemsCountPerProjectResult, LiveProjects_PluginsCount>(ignoreDisabled);

        return await query
            .OrderByDescending(result => result.ItemsCount)
            .ToListAsync();
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetStockDevicesCountPerProject(bool ignoreDisabled = false)
    {
        using var session = store.OpenAsyncSession();
        var query = session.GetIgnoreDisabledQuery<ItemsCountPerProjectResult, LiveProjects_StockDevicesCount>(ignoreDisabled);

        return await query
            .OrderByDescending(result => result.ItemsCount)
            .ToListAsync();
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetMaxForLiveDevicesCountPerProject(bool ignoreDisabled = false)
    {
        using var session = store.OpenAsyncSession();
        var query = session.GetIgnoreDisabledQuery<ItemsCountPerProjectResult, LiveProjects_MaxForLiveDevicesCount>(ignoreDisabled);
        return await query
            .OrderByDescending(result => result.ItemsCount)
            .ToListAsync();
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestTracksCount(int? limit = null)
    {
        using var session = store.OpenAsyncSession();
        return await session.Query<ItemsCountPerProjectResult, LiveProjects_TracksCount>()
            .OrderByDescending(result => result.ItemsCount)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestPluginsCount(int? limit = null, bool ignoreDisabled = false)
    {
        using var session = store.OpenAsyncSession();
        var query = session.GetIgnoreDisabledQuery<ItemsCountPerProjectResult, LiveProjects_PluginsCount>(ignoreDisabled);

        return await query
            .OrderByDescending(result => result.ItemsCount)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedPlugins(int? limit = null, bool ignoreDisabled = false)
    {
        using var session = store.OpenAsyncSession();
        var query = session.GetIgnoreDisabledQuery<DevicesUsageCountResult, Plugins_ByUsageCount>(ignoreDisabled);

        return await query
            .OrderByDescending(result => result.UsageCount)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedStockDevices(int? limit = null, bool ignoreDisabled = false)
    {
        using var session = store.OpenAsyncSession();
        var query = session.GetIgnoreDisabledQuery<DevicesUsageCountResult, StockDevices_ByUsageCount>(ignoreDisabled);

        return await query
            .OrderByDescending(result => result.UsageCount)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedMaxForLiveDevices(int? limit, bool ignoreDisabled)
    {
        using var session = store.OpenAsyncSession();
        var query = session.GetIgnoreDisabledQuery<DevicesUsageCountResult, MaxForLiveDevices_ByUsageCount>(ignoreDisabled);
        return await query
            .OrderByDescending(result => result.UsageCount)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<IEnumerable<PluginDevice>> GetAllPluginsFromProjects(int? limit = null, bool ignoreDisabled = false)
    {
        using var session = store.OpenAsyncSession();
        var query = session.GetIgnoreDisabledQuery<AllDevices.Result, AllDevices>(ignoreDisabled);

        return await query
            .Where(result => result.Type == DeviceType.Plugin)
            .OrderByDescending(result => result.DeviceName)
            .Select(result => (PluginDevice)result.Device)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<IEnumerable<StockDevice>> GetAllStockDevicesFromProjects(int? limit = null, bool ignoreDisabled = false)
    {
        using var session = store.OpenAsyncSession();
        var query = session.GetIgnoreDisabledQuery<AllDevices.Result, AllDevices>(ignoreDisabled);

        return await query
            .Where(result => result.Type == DeviceType.Stock)
            .OrderByDescending(result => result.DeviceName)
            .Select(result => (StockDevice)result.Device)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<IEnumerable<MaxForLiveDevice>> GetAllMaxForLiveDevicesFromProjects(int? limit = null, bool ignoreDisabled = false)
    {
        using var session = store.OpenAsyncSession();
        var query = session.GetIgnoreDisabledQuery<AllDevices.Result, AllDevices>(ignoreDisabled);

        return await query
            .Where(result => result.Type == DeviceType.MaxForLive)
            .OrderByDescending(result => result.DeviceName)
            .Select(result => (MaxForLiveDevice)result.Device)
            .Limit(limit)
            .ToListAsync();
    }


}
