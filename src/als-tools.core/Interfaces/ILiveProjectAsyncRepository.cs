using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncRepository
{
    Task InsertAsync(LiveProject project);

    Task InsertAsync(IEnumerable<LiveProject> projects);
    
    Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate);

    Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync();
    
    Task DeleteAllAsync();
    
    Task<int> CountProjectsAsync();

    Task<IEnumerable<TracksCountPerProjectResult>> GetTracksCountPerProjectAsync(int limit);
}
