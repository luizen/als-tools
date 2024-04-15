using AlsTools.Core.Interfaces;

namespace AlsTools.Infrastructure.FileSystem;

public class LiveProjectFileSystem : ILiveProjectFileSystem
{
    private readonly UserFolderHandler userFolderHandler;

    public LiveProjectFileSystem(UserFolderHandler userFolderHandler)
    {
        this.userFolderHandler = userFolderHandler;
    }

    public IReadOnlyList<string> GetProjectFilesFullPathFromDirectories(IEnumerable<string> folderPaths, bool includeBackupFolder)
    {
        var result = new List<string>();

        foreach (var folderPath in folderPaths)
        {
            var fileFullPaths = GetProjectFilesFullPathFromSingleDirectory(folderPath, includeBackupFolder);
            result.AddRange(fileFullPaths);
        }

        return result;
    }

    public IReadOnlyList<string> GetProjectFilesFullPathFromSetFiles(IEnumerable<string> setFilePaths)
    {
        var result = new List<string>();

        foreach (var filePath in setFilePaths)
        {
            var file = GetProjectFileFromSetFile(filePath);
            result.Add(file.FullName);
        }

        return result;
    }

    private IReadOnlyCollection<string> GetProjectFilesFullPathFromSingleDirectory(string folderPath, bool includeBackupFolder)
    {
        var path = userFolderHandler.GetFullPath(folderPath);
        var fullPaths = MultiEnumerateFiles(path, "*.als|*.alc");

        if (!includeBackupFolder)
            fullPaths = fullPaths.Where(path => !path.Contains(@"/Backup/", StringComparison.InvariantCultureIgnoreCase));

        return fullPaths.ToList();
    }

    private IEnumerable<string> MultiEnumerateFiles(string path, string patterns)
    {
        foreach (var pattern in patterns.Split('|'))
            foreach (var fileName in Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories))
                yield return fileName;
    }

    private FileInfo GetProjectFileFromSetFile(string setFilePath)
    {
        var path = userFolderHandler.GetFullPath(setFilePath);
        FileInfo file = new FileInfo(path);

        if (!file.Exists)
            throw new FileNotFoundException($"The specified file does not exist ({path})");

        return file;
    }
}
