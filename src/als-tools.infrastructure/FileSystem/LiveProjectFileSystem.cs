using AlsTools.Core.Interfaces;

namespace AlsTools.Infrastructure.FileSystem;

public class LiveProjectFileSystem : ILiveProjectFileSystem
{
    private readonly UserFolderHandler userFolderHandler;
    private readonly ILogger<LiveProjectFileSystem> logger;

    public LiveProjectFileSystem(UserFolderHandler userFolderHandler, ILogger<LiveProjectFileSystem> logger)
    {
        this.userFolderHandler = userFolderHandler;
        this.logger = logger;
    }

    public IEnumerable<string> GetProjectFilesFullPathFromPaths(IEnumerable<string> paths, bool includeBackupFolder = false)
    {
        var resultFiles = new List<string>();

        foreach (var path in paths)
        {
            var fullPath = userFolderHandler.GetFullPath(path);

            if (PathHelper.IsFile(fullPath))
            {
                resultFiles.Add(fullPath);
            }
            else if (PathHelper.IsDirectory(fullPath))
            {
                var fileFullPaths = GetAllProjectFilesFromDirectory(fullPath, includeBackupFolder);
                resultFiles.AddRange(fileFullPaths);
            }
            else
            {
                logger.LogError("The specified path either does not exist or is not a file nor directory: {@FullPath}", fullPath);
            }
        }

        return resultFiles;
    }

    private IEnumerable<string> GetAllProjectFilesFromDirectory(string folderPath, bool includeBackupFolder)
    {
        var allFilePaths = MultiEnumerateFiles(folderPath, "*.als|*.alc");

        if (!includeBackupFolder)
            allFilePaths = allFilePaths.Where(path => !path.Contains(@"/Backup/", StringComparison.InvariantCultureIgnoreCase));

        return allFilePaths.ToList();
    }

    private IEnumerable<string> MultiEnumerateFiles(string path, string patterns)
    {
        foreach (var pattern in patterns.Split('|'))
            foreach (var fileName in Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories))
                yield return fileName;
    }
}
