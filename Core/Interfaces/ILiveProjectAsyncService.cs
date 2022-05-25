using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncService
{
    Task<int> InitializeDbFromFilesAsync(IEnumerable<string> filePaths);

    Task<int> InitializeDbFromFoldersAsync(IEnumerable<string> folderPaths, bool includeBackupFolder);
    
    Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync();
    
    Task<IReadOnlyList<LiveProject>> GetProjectsContainingPluginsAsync(IEnumerable<string> pluginsToLocate);
    
    Task<int> CountProjectsAsync();
}
