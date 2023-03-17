using System.Linq.Expressions;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using Raven.Client.Documents;
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

    public Task<IReadOnlyList<LiveProject>> Search(Expression<Func<LiveProject, bool>> filter)
    {
        throw new NotImplementedException();
    }
}
