using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using AlsTools.Core.Queries;
using AlsTools.Infrastructure.Indexes;
using AlsTools.Infrastructure.Specifications;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Operations;

namespace AlsTools.Infrastructure.Repositories;

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




    public async Task<IReadOnlyList<LiveProject>> Search(QuerySpecification specification)
    {
        if (specification == null)
            throw new ArgumentNullException(nameof(specification));

        using (var session = store.OpenAsyncSession())
        {

            var projNameSpec = new ProjectNameSpecification("AllLiveStockDevices.als");
            var expression = projNameSpec.ToExpression();
            // var mv = "5";
            // var projMajorVersionSpec = new ProjectMajorVersionSpecification(mv);
            // var querySpec = new AndSpecification<LiveProject>(projNameSpec, projMajorVersionSpec);

            var results7 = await session.Query<LiveProject, LiveProjects_ByFullSearch>()
                            .Where(expression).ToListAsync();

            return results7;


// var query = session.Query<LiveProject, LiveProjects_ByFullSearch>();//.Where(project => project.Tracks.Any(track => track.Plugins.Any(plugin => plugin.Name == name)));

            // Expression<Func<LiveProject, bool>> predicate;

            // GetPluginQueryPredicate(predicate, specification.PluginQuery);

            // GetPluginQueryFilters(query, specification.PluginQuery);

            // string name = specification.PluginQuery.Names.First();
            // var query2 = session.Query<LiveProject, LiveProjects_ByFullSearch>()
            //     .Where(project => project
            //         .Tracks.Any(track => track
            //             .Plugins.Any(plugin =>
            //                 plugin.Name.In(specification.PluginQuery!.Names) &&
            //                 plugin.Format.In(specification.PluginQuery.Formats)
            //             )
            //         )
            //     );

            // ==> FUNCIONA
            // var projNameSpec = new ProjectNameSpecification("AllLiveStockDevices.als");
            // var mv = "5";
            // var projMajorVersionSpec = new ProjectMajorVersionSpecification(mv);
            // var querySpec = new AndSpecification<LiveProject>(projNameSpec, projMajorVersionSpec);

            // var query = querySpec.SatisfyingElementsFrom(session.Query<LiveProject>());
            // var results6 = await query.ToListAsync();

            // return results6!;
            // ==> FUNCIONA

            // var results3 = await session.Query<LiveProject, LiveProjects_ByFullSearch>().Where(spec.ToExpression()).ToListAsync();
            // var results3 = await session.Query<LiveProject>().Where(spec.ToExpression()).ToListAsync();


            // var results4 = await session.Query<LiveProject, LiveProjects_ByFullSearch>().Where(spec.ToExpression()).ToListAsync();

            // await session.Advanced.AsyncDocumentQuery<LiveProjects_ByFullSearch.Result, LiveProjects_ByFullSearch>().Where(spec.ToExpression()).

            // session.Query

            // return results4;

            // var results5 = await session.Query<LiveProjects_ByFullSearch.Result, LiveProjects_ByFullSearch>()
            //          .Where(x => spec.IsSatisfiedBy(x))
            //          .ToListAsync();

            // return results5!;



            // var query3 = session.Query<LiveProject, LiveProjects_ByFullSearch>()
            //     .Where(((specification.IsSatisfiedBy).ToList();


            //     using (var session = documentStore.OpenSession())
            //     {
            //         var spec1 = new UserSpecification("John Doe");
            // var spec2 = new UserSpecification("Jane Doe");
            // var compositeSpec = new CompositeSpecification<User>(spec1, spec2, Expression.And);
            // var users = session.Query<User>().Where(compositeSpec.ToExpression()).ToList();
            //     }

            // var results = await query.ToListAsync();
            // var results2 = await query2.ToListAsync();

            // return results2;
        }
    }

    //  &&
    //                             plugin.Family.Sort.In(specification.PluginQuery.Sorts)


    // private void GetPluginQueryPredicate(Expression<Func<LiveProject, bool>> predicate, PluginQuery? pluginQuery)
    // {
    //     if (pluginQuery == null)
    //         return;

    //     if (pluginQuery.Names.Any())
    //     {
    //         string name = pluginQuery.Names.First();
    //         if (predicate == null)
    //             predicate = project => project.Tracks.Any(track => track.Plugins.Any(plugin => plugin.Name == name));
    //         else
    //             Expression.And(predicate, Expression.New())
    //     }
    // }

    // private void GetPluginQueryFilters(IRavenQueryable<LiveProject> query, PluginQuery? pluginQuery)
    // {
    //     if (pluginQuery == null)
    //         return;

    //     if (pluginQuery.Names.Any())
    //     {
    //         string name = pluginQuery.Names.First();
    //         query.Where(project => project.Tracks.Any(track => track.Plugins.Any(plugin => plugin.Name == name)));
    //     }

    //     // if (pluginQuery.Formats.Any())
    //     //     query = query.Where(plugin => plugin.Format.In(pluginQuery.Formats));
    // }

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
}
