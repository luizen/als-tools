using System.Collections.Generic;
using System.IO;

namespace AlsTools.Core.Interfaces
{
    public interface ILiveProjectFileSystem
    {
        IEnumerable<FileInfo> LoadProjectFilesFromDirectories(IEnumerable<string> folderPaths, bool includeBackupFolder);

        IEnumerable<FileInfo> LoadProjectFilesFromSetFiles(IEnumerable<string> setFilePaths);
    }
}
