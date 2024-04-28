using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
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
        await using (var bulkInsert = store.BulkInsert())
        {
            int count = 0;
            foreach (var project in projects)
            {
                await bulkInsert.StoreAsync(project);
                count++;
            }

            logger.LogDebug("Inserted {InsertedProjects} projects", count);
        }
    }

    public async Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate)
    {
        using (var session = store.OpenAsyncSession())
        {
            var results = await session
                .Query<LiveProject, LiveProjects_ByPluginNames>()
                .Where(plugin => plugin.Name.In(pluginsToLocate))
                .ToListAsync();

            return results;
        }
    }

    public async Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync()
    {
        using (var session = store.OpenAsyncSession())
        {
            return await session.Query<LiveProject>().ToListAsync();
        }
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
        using (var session = store.OpenAsyncSession())
        {
            return await session.Query<LiveProject>().CountAsync();
        }
    }

    public async Task<IEnumerable<TracksCountPerProjectResult>> GetTracksCountPerProject()
    {
        using (var session = store.OpenAsyncSession())
        {
            return await session.Query<TracksCountPerProjectResult, LiveProjects_TracksCount>()
                .OrderBy(result => result.ProjectName)
                .ToListAsync();
        }
    }

    public async Task<IEnumerable<PluginsCountPerProjectResult>> GetPluginsCountPerProject(bool ignoreDisabled)
    {
        using (var session = store.OpenAsyncSession())
        {
            var query = GetPluginsCountDisabledQuery(ignoreDisabled);

            return await query
                .OrderByDescending(result => result.ProjectName)
                .ToListAsync();
        }
    }

    public async Task<IEnumerable<StockDevicesCountPerProjectResult>> GetStockDevicesCountPerProject(bool ignoreDisabled)
    {
        using (var session = store.OpenAsyncSession())
        {
            var query = GetStockDevicesCountDisabledQuery(ignoreDisabled);

            return await query
                .OrderByDescending(result => result.ProjectName)
                .ToListAsync();
        }
    }

    public async Task<IEnumerable<TracksCountPerProjectResult>> GetProjectsWithHighestTracksCount(int limit)
    {
        using (var session = store.OpenAsyncSession())
        {
            return await session.Query<TracksCountPerProjectResult, LiveProjects_TracksCount>()
                .OrderByDescending(result => result.TracksCount)
                .Take(limit)
                .ToListAsync();
        }
    }

    public async Task<IEnumerable<PluginsCountPerProjectResult>> GetProjectsWithHighestPluginsCount(int limit, bool ignoreDisabled)
    {
        using (var session = store.OpenAsyncSession())
        {
            var query = GetPluginsCountDisabledQuery(ignoreDisabled);

            return await query
                .OrderByDescending(result => result.PluginsCount)
                .Take(limit)
                .ToListAsync();
        }
    }

    public async Task<IEnumerable<PluginsUsageCountResult>> GetMostUsedPlugins(int limit, bool ignoreDisabled)
    {
        using (var session = store.OpenAsyncSession())
        {
            var query = ignoreDisabled
                ? store.OpenAsyncSession().Query<PluginsUsageCountResult, Plugins_ByUsageCount_EnabledOnly>()
                : store.OpenAsyncSession().Query<PluginsUsageCountResult, Plugins_ByUsageCount>();

            var results = await query
                .OrderByDescending(result => result.UsageCount)
                .Take(limit)
                .ToListAsync();

            return results;
        }
    }

    public async Task<IEnumerable<StockDevicesUsageCountResult>> GetMostUsedStockDevices(int limit, bool ignoreDisabled)
    {
        using (var session = store.OpenAsyncSession())
        {
            var query = ignoreDisabled
                ? store.OpenAsyncSession().Query<StockDevicesUsageCountResult, Plugins_ByUsageCount_EnabledOnly>()
                : store.OpenAsyncSession().Query<StockDevicesUsageCountResult, Plugins_ByUsageCount>();

            var results = await query
                .OrderByDescending(result => result.UsageCount)
                .Take(limit)
                .ToListAsync();

            return results;
        }
    }
}
