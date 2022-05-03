using System.Collections.Generic;
using System.Threading.Tasks;
using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces
{
    public interface ILiveProjectAsyncRepository
    {
        Task InsertAsync(LiveProject project);

        Task InsertAsync(IEnumerable<LiveProject> projects);
        
        Task<IEnumerable<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate);

        Task<IEnumerable<LiveProject>> GetAllProjectsAsync();
        
        Task DeleteAllAsync();
        
        Task<int> CountProjectsAsync();
    }
}
