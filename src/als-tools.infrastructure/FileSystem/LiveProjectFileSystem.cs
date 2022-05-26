using AlsTools.Core.Interfaces;

namespace AlsTools.Infrastructure.FileSystem;

public class LiveProjectFileSystem : ILiveProjectFileSystem
{
    private readonly UserFolderHandler userFolderHandler;

    public LiveProjectFileSystem(UserFolderHandler userFolderHandler)
    {
        this.userFolderHandler = userFolderHandler;
    }
    
    public IReadOnlyList<FileInfo> LoadProjectFilesFromDirectories(IEnumerable<string> folderPaths, bool includeBackupFolder)
    {
        var result = new List<FileInfo>();

        foreach (var folderPath in folderPaths)
        {
            var files = GetProjectFilesFromSingleDirectory(folderPath, includeBackupFolder);
            result.AddRange(files);
        }

        return result;
    }

    public IReadOnlyList<FileInfo> LoadProjectFilesFromSetFiles(IEnumerable<string> setFilePaths)
    {
        var result = new List<FileInfo>();

        foreach (var filePath in setFilePaths)
        {
            var file = GetProjectFileFromSetFile(filePath);
            result.Add(file);
        }

        return result;
    }

    private IReadOnlyCollection<FileInfo> GetProjectFilesFromSingleDirectory(string folderPath, bool includeBackupFolder)
    {
        var path = userFolderHandler.GetFullPath(folderPath);
        var dirInfo = new DirectoryInfo(path);
        var files = dirInfo.GetFiles("*.als", new EnumerationOptions() { RecurseSubdirectories = true }).AsEnumerable();

        if (!includeBackupFolder)
            files = files.Where(x => !x.FullName.Contains(@"/Backup/", StringComparison.InvariantCultureIgnoreCase));

        return files.ToList();
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
