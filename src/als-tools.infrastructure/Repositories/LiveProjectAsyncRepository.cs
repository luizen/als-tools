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
        return await context.Projects.Include(p => p.Tracks).ToListAsync();
    }
}
