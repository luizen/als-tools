using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncRepository
{
    Task InsertAsync(LiveProject project);

    Task<List<LiveProject>> GetAllProjectsAsync();

    Task DeleteAllAsync();
}

