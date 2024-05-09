using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncRepository
{
    Task InsertAsync(LiveProject project);

    Task<IEnumerable<LiveProject>> GetAllProjectsAsync();

    Task DeleteAllAsync();
}

