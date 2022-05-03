using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlsTools.Core.Interfaces;

namespace AlsTools.Infrastructure.FileSystem
{
    public class LiveProjectFileSystem : ILiveProjectFileSystem
    {
        public IEnumerable<FileInfo> LoadProjectFilesFromDirectories(IEnumerable<string> folderPaths, bool includeBackupFolder)
        {
            var result = new List<FileInfo>();

            foreach (var folderPath in folderPaths)
            {
                var files = GetProjectFilesFromSingleDirectory(folderPath, includeBackupFolder);
                result.AddRange(files);
            }

            return result;
        }

        public IEnumerable<FileInfo> LoadProjectFilesFromSetFiles(IEnumerable<string> setFilePaths)
        {
            var result = new List<FileInfo>();

            foreach (var filePath in setFilePaths)
            {
                var file = GetProjectFileFromSetFile(filePath);
                result.Add(file);
            }

            return result;            
        }

        private IEnumerable<FileInfo> GetProjectFilesFromSingleDirectory(string folderPath, bool includeBackupFolder)
        {
            var d = new DirectoryInfo(folderPath);
            var files = d.GetFiles("*.als", new EnumerationOptions() { RecurseSubdirectories = true }).AsEnumerable();

            if (!includeBackupFolder)
                files = files.Where(x => !x.FullName.Contains(@"/Backup/", StringComparison.InvariantCultureIgnoreCase));

            return files;
        }
        
        private FileInfo GetProjectFileFromSetFile(string setFilePath)
        {
            FileInfo f = new FileInfo(setFilePath);

            if (!f.Exists)
                throw new FileNotFoundException($"The specified file does not exist ({setFilePath})");

            return f;
        }

    }
}
