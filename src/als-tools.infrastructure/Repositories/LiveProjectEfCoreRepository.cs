using AlsTools.Core;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlsTools.Infrastructure.Repositories;

public class LiveProjectEfCoreRepository : ILiveProjectAsyncRepository
{
    private readonly ILogger<LiveProjectEfCoreRepository> logger;
    private readonly AlsToolsDbContext dbContext;

    public LiveProjectEfCoreRepository(ILogger<LiveProjectEfCoreRepository> logger, AlsToolsDbContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }

    public async Task<int> CountProjectsAsync()
    {
        return await dbContext.LiveProjects.CountAsync();
    }

    public async Task DeleteAllAsync()
    {
        dbContext.LiveProjects.RemoveRange(dbContext.LiveProjects);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync()
    {
        return await dbContext.LiveProjects.ToListAsync();
    }

    public async Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate)
    {
        return await dbContext.LiveProjects
            .Where(project => project.Tracks.Any(track => track.Plugins.Any(plugin => pluginsToLocate.Contains(plugin.Name))))
            .ToListAsync();
    }

    public async Task<IEnumerable<NameCountElement>> GetTracksCountPerProjectAsync()
    {
        return await dbContext.LiveProjects
            .Select(project => new NameCountElement { Name = project.Name, Count = project.Tracks.Count })
            .ToListAsync();
    }

    public async Task InsertAsync(LiveProject project)
    {
        dbContext.LiveProjects.Add(project);
        await dbContext.SaveChangesAsync();

        logger.LogDebug("Inserted project {ProjectName}", project.Name);
    }

    public async Task InsertAsync(IEnumerable<LiveProject> projects)
    {
        dbContext.LiveProjects.AddRange(projects);
        await dbContext.SaveChangesAsync();

        logger.LogDebug("Inserted {Count} projects", projects.Count());
    }
}