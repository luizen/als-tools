using AlsTools.Core.Entities;
using AlsTools.Core.Queries;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncRepository
{
    Task InsertAsync(LiveProject project);

    Task InsertAsync(IEnumerable<LiveProject> projects);

    Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate);

    Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(QuerySpecification specification);

    Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync();

    Task DeleteAllAsync();

    Task<int> CountProjectsAsync();
}
