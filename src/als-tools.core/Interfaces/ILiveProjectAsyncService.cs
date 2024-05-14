using AlsTools.Core.Models;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncService
{

    Task<List<Project>> GetAllProjectsAsync();

    Task<Project?> GetProjectByIdAsync(int id);

    Task<int> InsertAsync(Project project);

    Task<int> DeleteAllAsync();

    Task<int> DeleteAsync(int id);

    Task<int> DeleteAsync(Project project);

    // Task DeleteAllAsync();
}
