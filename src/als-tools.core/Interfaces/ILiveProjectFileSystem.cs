namespace AlsTools.Core.Interfaces;

public interface ILiveProjectFileSystem
{
    IReadOnlyList<string> GetProjectFilesFullPathFromDirectories(IEnumerable<string> folderPaths, bool includeBackupFolder);

    IReadOnlyList<string> GetProjectFilesFullPathFromSetFiles(IEnumerable<string> setFilePaths);
}
