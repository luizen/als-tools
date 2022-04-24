using System;
using System.Collections.Generic;
using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces
{
    public interface ILiveProjectService
    {
        int InitializeDbFromFile(string filePath);

        int InitializeDbFromFolder(string folderPath, bool includeBackupFolder);
        
        IEnumerable<LiveProject> GetAllProjects();
        
        IEnumerable<LiveProject> GetProjectsContainingPlugins(string[] pluginsToLocate);
        
        int CountProjects();
    }
}
