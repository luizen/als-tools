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

    public async Task<int> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }

    public async Task<int> DeleteAsync(Project project)
    {
        return await repository.DeleteAsync(project);
    }

    public async Task<int> DeleteAllAsync()
    {
        return await repository.DeleteAllAsync();
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await repository.GetAllProjectsAsync();
    }

    public async Task<int> InsertAsync(Project project)
    {
        return await repository.InsertAsync(project);
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        return await repository.GetProjectByIdAsync(id);
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
