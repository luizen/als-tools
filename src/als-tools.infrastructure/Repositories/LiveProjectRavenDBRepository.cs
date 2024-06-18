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
        logger.LogTrace("Start: InsertAsync for {@ProjectName}", project.Name);

        using (var session = store.OpenAsyncSession())
        {
            await session.StoreAsync(project);
            await session.SaveChangesAsync();
        }

        logger.LogTrace("End: InsertAsync for {@ProjectName}", project.Name);
    }

    public async Task UpdateAsync(LiveProject project)
    {
        logger.LogTrace("Start: UpdateAsync for {@ProjectName}", project.Name);

        using (var session = store.OpenAsyncSession())
        {
            await session.StoreAsync(project);
            await session.SaveChangesAsync();
        }

        logger.LogTrace("End: UpdateAsync for {@ProjectName}", project.Name);
    }

    public async Task InsertAsync(IEnumerable<LiveProject> projects)
    {
        logger.LogTrace("Start: InsertAsync for list of projects");

        await using var bulkInsert = store.BulkInsert();
        int count = 0;
        foreach (var project in projects)
        {
            await bulkInsert.StoreAsync(project);
            count++;
        }

        logger.LogTrace("End: InsertAsync for list of projects");
        logger.LogDebug("Inserted {@InsertedProjects} projects", count);
    }

    public async Task<IEnumerable<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate)
    {
        logger.LogTrace("Start: GetProjectsContainingPluginsAsync for {@Plugins}", pluginsToLocate);

        try
        {
            using var session = store.OpenAsyncSession();
            return await session
                .Query<LiveProject, LiveProjects_ByPluginNames>()
                .Where(plugin => plugin.Name.In(pluginsToLocate))
                .ToListAsync();
        }
        finally
        {
            logger.LogTrace("End: GetProjectsContainingPluginsAsync for {@Plugins}", pluginsToLocate);
        }
    }

    public async Task<IEnumerable<LiveProject>> GetAllProjectsAsync(int? limit = null)
    {
        logger.LogTrace("Start: GetAllProjectsAsync");

        try
        {
            using var session = store.OpenAsyncSession();
            return await session.Query<LiveProject>()
                .OrderBy(p => p.Name)
                .Limit(limit)
                .ToListAsync();
        }
        finally
        {
            logger.LogTrace("End: GetAllProjectsAsync");
        }
    }

    public async Task<IEnumerable<LiveProjectWithChildrenCountsResult>> GetAllProjectsWithChildrenCountsAsync(int? limit = null)
    {
        logger.LogTrace("Start: GetAllProjectsWithChildrenCountsAsync");

        try
        {
            using var session = store.OpenAsyncSession();
            return await session.Query<LiveProjectWithChildrenCountsResult, LiveProject_WithChildrenCounts>()
                .Limit(limit)
                .ToListAsync();
        }
        finally
        {
            logger.LogTrace("End: GetAllProjectsWithChildrenCountsAsync");
        }
    }

    public async Task<LiveProject?> GetProjectByIdAsync(string id)
    {
        logger.LogTrace("Start: GetProjectByIdAsync for {@ProjectId}", id);

        try
        {
            using var session = store.OpenAsyncSession();
            return await session.LoadAsync<LiveProject>(id);
        }
        finally
        {
            logger.LogTrace("End: GetProjectByIdAsync for {@ProjectId}", id);
        }
    }

    public async Task DeleteAllAsync()
    {
        logger.LogTrace("Start: DeleteAllAsync");

        var operation = await store
            .Operations
            .SendAsync(new DeleteByQueryOperation($"from {collectionName}"));

        operation.WaitForCompletion(TimeSpan.FromSeconds(5));

        logger.LogTrace("End: DeleteAllAsync");
    }

    public async Task<int> CountProjectsAsync()
    {
        logger.LogTrace("Start: CountProjectsAsync");

        try
        {
            using var session = store.OpenAsyncSession();
            return await session.Query<LiveProject>().CountAsync();
        }
        finally
        {
            logger.LogTrace("End: CountProjectsAsync");
        }
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetTracksCountPerProject()
    {
        logger.LogTrace("Start: GetTracksCountPerProject");

        try
        {
            using var session = store.OpenAsyncSession();
            return await session.Query<ItemsCountPerProjectResult, LiveProjects_TracksCount>()
                .OrderByDescending(result => result.ItemsCount)
                .ToListAsync();
        }
        finally
        {
            logger.LogTrace("End: GetTracksCountPerProject");
        }
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetPluginsCountPerProject(bool ignoreDisabled = false)
    {
        logger.LogTrace("Start: GetPluginsCountPerProject");

        try
        {
            using var session = store.OpenAsyncSession();
            var query = session.GetIgnoreDisabledQuery<ItemsCountPerProjectResult, LiveProjects_PluginsCount>(ignoreDisabled);

            return await query
                .OrderByDescending(result => result.ItemsCount)
                .ToListAsync();
        }
        finally
        {
            logger.LogTrace("End: GetPluginsCountPerProject");
        }
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetStockDevicesCountPerProject(bool ignoreDisabled = false)
    {
        logger.LogTrace("Start: GetStockDevicesCountPerProject");

        try
        {
            using var session = store.OpenAsyncSession();
            var query = session.GetIgnoreDisabledQuery<ItemsCountPerProjectResult, LiveProjects_StockDevicesCount>(ignoreDisabled);

            return await query
                .OrderByDescending(result => result.ItemsCount)
                .ToListAsync();
        }
        finally
        {
            logger.LogTrace("End: GetStockDevicesCountPerProject");
        }
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetMaxForLiveDevicesCountPerProject(bool ignoreDisabled = false)
    {
        logger.LogTrace("Start: GetMaxForLiveDevicesCountPerProject");

        try
        {
            using var session = store.OpenAsyncSession();
            var query = session.GetIgnoreDisabledQuery<ItemsCountPerProjectResult, LiveProjects_MaxForLiveDevicesCount>(ignoreDisabled);

            return await query
                .OrderByDescending(result => result.ItemsCount)
                .ToListAsync();
        }
        finally
        {
            logger.LogTrace("End: GetMaxForLiveDevicesCountPerProject");
        }
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestTracksCount(int? limit = null)
    {
        logger.LogTrace("Start: GetProjectsWithHighestTracksCount");

        try
        {
            using var session = store.OpenAsyncSession();
            return await session.Query<ItemsCountPerProjectResult, LiveProjects_TracksCount>()
                .OrderByDescending(result => result.ItemsCount)
                .Limit(limit)
                .ToListAsync();
        }
        finally
        {
            logger.LogTrace("End: GetProjectsWithHighestTracksCount");
        }
    }

    public async Task<IEnumerable<ItemsCountPerProjectResult>> GetProjectsWithHighestPluginsCount(int? limit = null, bool ignoreDisabled = false)
    {
        logger.LogTrace("Start: GetProjectsWithHighestPluginsCount");

        try
        {
            using var session = store.OpenAsyncSession();
            var query = session.GetIgnoreDisabledQuery<ItemsCountPerProjectResult, LiveProjects_PluginsCount>(ignoreDisabled);

            return await query
                .OrderByDescending(result => result.ItemsCount)
                .Limit(limit)
                .ToListAsync();
        }
        finally
        {
            logger.LogTrace("End: GetProjectsWithHighestPluginsCount");
        }
    }

    public async Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedPlugins(int? limit = null, bool ignoreDisabled = false)
    {
        logger.LogTrace("Start: GetMostUsedPlugins");

        try
        {
            using var session = store.OpenAsyncSession();
            var query = session.GetIgnoreDisabledQuery<DevicesUsageCountResult, Plugins_ByUsageCount>(ignoreDisabled);

            return await query
                .OrderByDescending(result => result.UsageCount)
                .Limit(limit)
                .ToListAsync();
        }
        finally
        {
            logger.LogTrace("End: GetMostUsedPlugins");
        }
    }

    public async Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedStockDevices(int? limit = null, bool ignoreDisabled = false)
    {
        logger.LogTrace("Start: GetMostUsedStockDevices");

        try
        {
            using var session = store.OpenAsyncSession();
            var query = session.GetIgnoreDisabledQuery<DevicesUsageCountResult, StockDevices_ByUsageCount>(ignoreDisabled);

            return await query
                .OrderByDescending(result => result.UsageCount)
                .Limit(limit)
                .ToListAsync();
        }
        finally
        {
            logger.LogTrace("End: GetMostUsedStockDevices");
        }
    }

    public async Task<IEnumerable<DevicesUsageCountResult>> GetMostUsedMaxForLiveDevices(int? limit, bool ignoreDisabled)
    {
        logger.LogTrace("Start: GetMostUsedMaxForLiveDevices");

        try
        {
            using var session = store.OpenAsyncSession();
            var query = session.GetIgnoreDisabledQuery<DevicesUsageCountResult, MaxForLiveDevices_ByUsageCount>(ignoreDisabled);

            return await query
                .OrderByDescending(result => result.UsageCount)
                .Limit(limit)
                .ToListAsync();
        }
        finally
        {
            logger.LogTrace("End: GetMostUsedMaxForLiveDevices");
        }
    }

    public async Task<IEnumerable<PluginDevice>> GetAllPluginsFromProjects(int? limit = null, bool ignoreDisabled = false)
    {
        logger.LogTrace("Start: GetAllPluginsFromProjects");

        try
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
        finally
        {
            logger.LogTrace("End: GetAllPluginsFromProjects");
        }
    }

    public async Task<IEnumerable<StockDevice>> GetAllStockDevicesFromProjects(int? limit = null, bool ignoreDisabled = false)
    {
        logger.LogTrace("Start: GetAllStockDevicesFromProjects");

        try
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
        finally
        {
            logger.LogTrace("End: GetAllStockDevicesFromProjects");
        }
    }

    public async Task<IEnumerable<MaxForLiveDevice>> GetAllMaxForLiveDevicesFromProjects(int? limit = null, bool ignoreDisabled = false)
    {
        logger.LogTrace("Start: GetAllMaxForLiveDevicesFromProjects");

        try
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
        finally
        {
            logger.LogTrace("End: GetAllMaxForLiveDevicesFromProjects");
        }
    }


}
