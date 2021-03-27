using System.Collections.Generic;
using System.IO;

namespace AlsTools.Core.Interfaces
{
    public interface ILiveProjectFileSystem
    {
        IEnumerable<FileInfo> LoadProjectFilesFromDirectory(string folderPath, bool includeBackupFolder);

        FileInfo LoadProjectFileFromSetFile(string setFilePath);
    }
}
