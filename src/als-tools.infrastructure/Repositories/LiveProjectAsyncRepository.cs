using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

public class LiveProjectAsyncRepository : ILiveProjectAsyncRepository
{
    private readonly AlsToolsContext context;

    public LiveProjectAsyncRepository(AlsToolsContext context)
    {
        this.context = context;
    }

    public Task DeleteAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<LiveProject>> GetAllProjectsAsync()
    {
        return context.LiveProjects.ToListAsync();
    }

    public Task InsertAsync(LiveProject project)
    {
        throw new NotImplementedException();
    }
}
