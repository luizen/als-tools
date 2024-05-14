using AlsTools.Core.Interfaces;
using AlsTools.Core.Models;
using AlsTools.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

public class LiveProjectAsyncRepository : ILiveProjectAsyncRepository
{
    private readonly AlsToolsDbContext context;

    public LiveProjectAsyncRepository(AlsToolsDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Project>> GetAllProjectsAsync(bool includeDependencies)
    {
        if (!includeDependencies)
        {
            return await context.Projects.ToListAsync();
        }

        return await context.Projects
            .Include(p => p.Tracks)
            .ThenInclude(t => t.Devices)
            .ToListAsync();
    }

    public async Task<int> InsertAsync(Project project)
    {
        context.Projects.Add(project);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteAllAsync()
    {
        // context.Projects.RemoveRange(context.Projects);
        // return await context.SaveChangesAsync();

        return await context.Projects.ExecuteDeleteAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        var project = await context.Projects.FindAsync(id);
        if (project != null)
        {
            context.Projects.Remove(project);
            return await context.SaveChangesAsync();
        }

        return 0;
    }

    public async Task<int> DeleteAsync(Project project)
    {
        context.Projects.Remove(project);

        return await context.SaveChangesAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        return await context.Projects.FindAsync(id);
    }
}
