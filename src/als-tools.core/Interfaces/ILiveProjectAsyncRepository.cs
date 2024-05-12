using AlsTools.Core.Models;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncRepository
{
    Task<int> DeleteAllAsync();
    Task<List<Project>> GetAllProjectsAsync();
    Task<int> InsertAsync(Project project);

    // Task InsertAsync(LiveProject project);

    // Task<List<LiveProject>> GetAllProjectsAsync();

    // Task DeleteAllAsync();
}

