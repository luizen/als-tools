using AlsTools.Core.Interfaces;
using AlsTools.Core.Models;
using AlsTools.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

public class LiveProjectAsyncRepository : ILiveProjectAsyncRepository
{
    private readonly MyNewDbContext context;

    public LiveProjectAsyncRepository(MyNewDbContext context)
    {
        this.context = context;
    }

    // public Task DeleteAllAsync()
    // {
    //     throw new NotImplementedException();
    // }

    // public Task<List<LiveProject>> GetAllProjectsAsync()
    // {
    //     return context.LiveProjects.ToListAsync();
    // }

    // public Task InsertAsync(LiveProject project)
    // {
    //     throw new NotImplementedException();
    // }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await context.Projects
            .Include(p => p.Tracks)
                .ThenInclude(t => t.PluginDevices)
            .Include(p => p.Tracks)
                .ThenInclude(t => t.StockDevices)
            .Include(p => p.Tracks)
                .ThenInclude(t => t.MaxForLiveDevices)
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
