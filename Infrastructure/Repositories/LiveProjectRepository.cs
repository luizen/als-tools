using System;
using System.Collections.Generic;
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

        public IEnumerable<LiveProject> GetAllProjects()
        {
            return liteDb.GetCollection<LiveProject>("LiveProject").FindAll();
        }

        public IEnumerable<LiveProject> GetProjectsContainingPlugins(string[] pluginsToLocate)
        {
            // var col = liteDb.GetCollection<LiveProject>("LiveProject");
            // var pluginsList = pluginsToLocate

            // // var projects = col.Query()
            // //     .Where(x => plu x.Plugins.("J"))
            // //     .OrderBy(x => x.Name)
            // //     .Select(x => new { x.Name, NameUpper = x.Name.ToUpper() })
            // //     .Limit(10)
            // //     .ToList();
            // var projects = col
            //     .Include(x => x.Plugins)
            //     .

            // return projects;

            throw new NotImplementedException();
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
