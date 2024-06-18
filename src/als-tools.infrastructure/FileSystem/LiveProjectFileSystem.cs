using AlsTools.Core.Entities;
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
                logger.LogDebug("Path {@Path} is a file", fullPath);
                resultFiles.Add(fullPath);
            }
            else if (PathHelper.IsDirectory(fullPath))
            {
                logger.LogDebug("Path {@Path} is a directory", fullPath);
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

    /// <summary>
    /// Loads and sets the file dates for the project.
    /// </summary>
    public void SetFileDates(LiveProject project)
    {
        var fileInfo = new FileInfo(project.Path);
        project.CreationTime = fileInfo.CreationTime;
        project.LastModified = fileInfo.LastWriteTime;
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
            {
                logger.LogDebug("Found file {@File} in folder {@Folder}", fileName, path);
                yield return fileName;
            }

    }
}
