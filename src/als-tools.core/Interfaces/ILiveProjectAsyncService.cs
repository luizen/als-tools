using AlsTools.Core.Models;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncService
{
    Task<List<Project>> GetAllProjectsAsync();

    // Task<IEnumerable<LiveProject>> GetAllProjectsAsync();

    // Task InsertAsync(LiveProject project);

    // Task DeleteAllAsync();
}
