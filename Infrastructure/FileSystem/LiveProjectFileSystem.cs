using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlsTools.Core.Interfaces;

namespace AlsTools.Infrastructure.FileSystem
{
    public class LiveProjectFileSystem : ILiveProjectFileSystem
    {
        public IEnumerable<FileInfo> LoadProjectFilesFromDirectory(string folderPath, bool includeBackupFolder)
        {
            var d = new DirectoryInfo(folderPath);
            var files = d.GetFiles("*.als", new EnumerationOptions() { RecurseSubdirectories = true }).AsEnumerable();

            if (!includeBackupFolder)
                files = files.Where(x => !x.FullName.Contains(@"\backup\"));

            return files;
        }
        public FileInfo LoadProjectFileFromSetFile(string setFilePath)
        {
            FileInfo f = new FileInfo(setFilePath);

            if (!f.Exists)
                throw new FileNotFoundException($"The specified file does not exist ({setFilePath})");

            return f;
        }

    }
}
