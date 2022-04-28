using System.Collections.Generic;
using System.Threading.Tasks;
using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces
{
    public interface ILiveProjectAsyncService
    {
        Task<int> InitializeDbFromFileAsync(string filePath);

        Task<int> InitializeDbFromFolderAsync(string folderPath, bool includeBackupFolder);
        
        Task<IEnumerable<LiveProject>> GetAllProjectsAsync();
        
        Task<IEnumerable<LiveProject>> GetProjectsContainingPluginsAsync(string[] pluginsToLocate);
        
        Task<int> CountProjectsAsync();
    }
}
