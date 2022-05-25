namespace AlsTools.Core.Interfaces;

public interface ILiveProjectFileSystem
{
    IReadOnlyList<FileInfo> LoadProjectFilesFromDirectories(IEnumerable<string> folderPaths, bool includeBackupFolder);

    IReadOnlyList<FileInfo> LoadProjectFilesFromSetFiles(IEnumerable<string> setFilePaths);
}
