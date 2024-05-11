using AlsTools.Core.Models;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncRepository
{
    Task<List<Project>> GetAllProjectsAsync();
    Task InsertAsync(Project project);

    // Task InsertAsync(LiveProject project);

    // Task<List<LiveProject>> GetAllProjectsAsync();

    // Task DeleteAllAsync();
}

