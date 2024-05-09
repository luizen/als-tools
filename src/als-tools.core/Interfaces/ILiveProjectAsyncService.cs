using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncService
{
    Task<IEnumerable<LiveProject>> GetAllProjectsAsync();

    Task InsertAsync(LiveProject project);

    Task DeleteAllAsync();
}
