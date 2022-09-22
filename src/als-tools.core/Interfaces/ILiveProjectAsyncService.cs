using AlsTools.Core.Entities;
using AlsTools.Core.Queries;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncService
{
    Task<int> InitializeDbFromFilesAsync(IEnumerable<string> filePaths);

    Task<int> InitializeDbFromFoldersAsync(IEnumerable<string> folderPaths, bool includeBackupFolder);

    Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync();

    Task<IReadOnlyList<LiveProject>> Search(QuerySpecification specification);

    Task<int> CountProjectsAsync();
}
