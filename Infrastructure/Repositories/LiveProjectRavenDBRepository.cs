using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using AlsTools.Infrastructure.Indexes;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;

namespace AlsTools.Infrastructure.Repositories
{
    public class LiveProjectRavenRepository : ILiveProjectAsyncRepository
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
            BulkInsertOperation bulkInsert = null;

            try
            {
                bulkInsert = store.BulkInsert();
                int count = 0;
                foreach (var project in projects)
                {
                    await bulkInsert.StoreAsync(project);
                    count++;
                }

                logger.LogDebug("Inserted {InsertedProjects} projects", count);
            }
            finally
            {
                if (bulkInsert != null)
                    await bulkInsert.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<LiveProject>> GetProjectsContainingPluginsAsync(string[] pluginsToLocate)
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

        public async Task<IEnumerable<LiveProject>> GetAllProjectsAsync()
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
    }
}
