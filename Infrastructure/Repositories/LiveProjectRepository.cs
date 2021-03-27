using System;
using System.Collections.Generic;
using System.Linq;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace AlsTools.Infrastructure.Repositories
{
    public class LiveProjectRepository : ILiveProjectRepository
    {
        private readonly LiteDatabase liteDb;
        private readonly ILogger<LiveProjectRepository> logger;
        private readonly ILiteDbContext dbContext;

        public LiveProjectRepository(ILogger<LiveProjectRepository> logger, ILiteDbContext dbContext)
        {
            this.liteDb = dbContext.Database;
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public void DeleteAll()
        {
            var deletedCount = liteDb.GetCollection<LiveProject>("LiveProject").DeleteAll();
            logger.LogDebug("Deleted {DeletedCount} projects", deletedCount);
        }

        public IEnumerable<LiveProject> GetAllProjects()
        {
            return liteDb.GetCollection<LiveProject>("LiveProject").FindAll();
        }

        public IEnumerable<LiveProject> GetProjectsContainingPlugins(string[] pluginsToLocate)
        {
            // var col = liteDb.GetCollection<LiveProject>("LiveProject");

            // var pluginsList = pluginsToLocate

            // var projects = col
            //     .Query()
            //     .Where(proj => proj.Plugins != null && proj.Plugins.Any(k => pluginsToLocate.Any(x => k.Name.Contains(x, StringComparison.InvariantCultureIgnoreCase))))
            //     .Select(p => p)
            //     .ToEnumerable();

            // var projects = col
            //     .Query()
            //     .Where(proj => proj.Plugins.Where(plugin => pluginsToLocate.Contains(plugin.Name)).Any())
            //     .Select(p => p)
            //     .ToEnumerable();

            // var projects = col
            //     .Query()
            //     .Where(proj => 
            //         proj.Plugins.Where(plugin => 
            //             pluginsToLocate.Any(p => p.Contains(plugin.Name, StringComparison.InvariantCultureIgnoreCase))
            //         ).Any()
            //     )
            //     .Select(p => p)
            //     .ToEnumerable();

            // var projects = col
            //     .Include(x => x.Plugins)
            //     .FindAll()
            //     .Where(p => p.Plugins.Intersect(pluginsToLocate))
                
            var projects = GetAllProjects();
            IList<LiveProject> res = new List<LiveProject>();
            foreach (var p in projects)
            {                
                if (p.Plugins.Any(x => pluginsToLocate.Any(y => x.Key.Contains(y, StringComparison.InvariantCultureIgnoreCase))))
                    res.Add(p);   
            }

            return res.AsEnumerable();
        }

        public bool Insert(LiveProject project)
        {
            var col = liteDb.GetCollection<LiveProject>("LiveProject");
            var res = col.Insert(project);

            // Create an index over the Name property (if it doesn't exist)
            col.EnsureIndex(x => x.Name);

            logger.LogDebug("Insert result {Result}", res);

            return res;
        }

        public int Insert(IEnumerable<LiveProject> projects)
        {
            var col = liteDb.GetCollection<LiveProject>("LiveProject");
            var res = col.InsertBulk(projects);

            // Create an index over the Name property (if it doesn't exist)
            col.EnsureIndex(x => x.Name);

            logger.LogDebug("Inserted {InsertedProjects} projects", res);

            return res;
        }
    }
}
