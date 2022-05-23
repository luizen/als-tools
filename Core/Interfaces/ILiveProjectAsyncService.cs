using System.Collections.Generic;
using System.Threading.Tasks;
using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncService
{
    Task<int> InitializeDbFromFilesAsync(IEnumerable<string> filePaths);

    Task<int> InitializeDbFromFoldersAsync(IEnumerable<string> folderPaths, bool includeBackupFolder);
    
    Task<IEnumerable<LiveProject>> GetAllProjectsAsync();
    
    Task<IEnumerable<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate);
    
    Task<int> CountProjectsAsync();
}
