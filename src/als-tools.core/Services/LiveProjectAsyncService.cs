using AlsTools.Core.Interfaces;
using AlsTools.Core.Models;

namespace AlsTools.Core.Services;

public class LiveProjectAsyncService : ILiveProjectAsyncService
{
    private readonly ILogger<LiveProjectAsyncService> logger;
    private readonly ILiveProjectAsyncRepository repository;

    public LiveProjectAsyncService(ILogger<LiveProjectAsyncService> logger, ILiveProjectAsyncRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await repository.GetAllProjectsAsync();
    }

    public async Task InsertAsync(Project project)
    {
        await repository.InsertAsync(project);
    }


    // public async Task<IEnumerable<LiveProject>> GetAllProjectsAsync()
    // {
    //     return await repository.GetAllProjectsAsync();
    // }

    // public async Task InsertAsync(LiveProject project)
    // {
    //     await repository.InsertAsync(project);
    // }

    // public async Task DeleteAllAsync()
    // {
    //     await repository.DeleteAllAsync();
    // }
}
