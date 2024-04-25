// using AlsTools.Core;
// using AlsTools.Core.Entities;
// using AlsTools.Core.Interfaces;
// using AlsTools.Infrastructure.Indexes;
// using Raven.Client.Documents;
// using Raven.Client.Documents.BulkInsert;
// using Raven.Client.Documents.Linq;
// using Raven.Client.Documents.Operations;

// namespace AlsTools.Infrastructure.Repositories;

// public class LiveProjectRavenRepository : ILiveProjectAsyncRepository
// {
//     private readonly ILogger<LiveProjectRavenRepository> logger;
//     private readonly IEmbeddedDatabaseContext dbContext;
//     private readonly IDocumentStore store;
//     private readonly string collectionName;

//     public LiveProjectRavenRepository(ILogger<LiveProjectRavenRepository> logger, IEmbeddedDatabaseContext dbContext)
//     {
//         this.logger = logger;
//         this.dbContext = dbContext;
//         this.store = dbContext.DocumentStore;
//         this.collectionName = store.Conventions.GetCollectionName(typeof(LiveProject));
//     }

//     public async Task InsertAsync(LiveProject project)
//     {
//         using (var session = store.OpenAsyncSession())
//         {
//             await session.StoreAsync(project);
//             await session.SaveChangesAsync();
//         }

//         logger.LogDebug("Inserted project {ProjectName}", project.Name);
//     }

//     public async Task InsertAsync(IEnumerable<LiveProject> projects)
//     {
//         await using (var bulkInsert = store.BulkInsert())
//         {
//             int count = 0;
//             foreach (var project in projects)
//             {
//                 await bulkInsert.StoreAsync(project);
//                 count++;
//             }

//             logger.LogDebug("Inserted {InsertedProjects} projects", count);
//         }
//     }

//     public async Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate)
//     {
//         using (var session = store.OpenAsyncSession())
//         {
//             var results = await session
//                 .Query<LiveProject, LiveProjects_ByPluginNames>()
//                 .Where(plugin => plugin.Name.In(pluginsToLocate))
//                 .ToListAsync();

//             return results;
//         }
//     }

//     public async Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync()
//     {
//         using (var session = store.OpenAsyncSession())
//         {
//             return await session.Query<LiveProject>().ToListAsync();
//         }
//     }

//     public async Task DeleteAllAsync()
//     {
//         var operation = await store
//             .Operations
//             .SendAsync(new DeleteByQueryOperation($"from {collectionName}"));

//         operation.WaitForCompletion(TimeSpan.FromSeconds(5));
//     }

//     public async Task<int> CountProjectsAsync()
//     {
//         using (var session = store.OpenAsyncSession())
//         {
//             return await session.Query<LiveProject>().CountAsync();
//         }
//     }

//     public async Task<IEnumerable<NameCountElement>> GetTracksCountPerProjectAsync()
//     {
//         using var session = store.OpenAsyncSession();

//         // return await session.Query<NameCountElement, LiveProjects_TrackCount>()
//         //     .OrderByDescending(x => x.Count)
//         //     .Take(10)
//         //     .ToListAsync();

//         var projectPluginCounts = await session.Query<LiveProject>()
//            .Select(p => new NameCountElement()
//            {
//                Name = p.Name,
//                Count = p.Tracks.Sum(t => t.Plugins.Count)
//            })
//            .OrderByDescending(p => p.Count)
//            .ToListAsync();

//         return projectPluginCounts;
//     }
// }
