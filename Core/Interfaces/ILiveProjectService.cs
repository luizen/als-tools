using System;
using System.Collections.Generic;
using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces
{
    public interface ILiveProjectService
    {
        void InitializeDbFromFile(string filePath);

        void InitializeDbFromFolder(string folderPath, bool includeBackupFolder);
        
        IList<LiveProject> GetAllProjects();
        
        IList<LiveProject> GetProjectsContainingPlugins(string[] pluginsToLocate);
    }
}
