namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncService
{
    Task<int> InitializeDbFromFilesAsync(IEnumerable<string> filePaths);

    Task<int> InitializeDbFromFoldersAsync(IEnumerable<string> folderPaths, bool includeBackupFolder);

    Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync();

    Task<IReadOnlyList<LiveProject>> Search(FilterContext filterContext);

    Task<int> CountProjectsAsync();
}
