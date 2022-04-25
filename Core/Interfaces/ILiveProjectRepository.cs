using System.Collections.Generic;
using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces
{
    public interface ILiveProjectRepository
    {
        bool Insert(LiveProject project);

        int Insert(IEnumerable<LiveProject> projects);
        
        IEnumerable<LiveProject> GetProjectsContainingPlugins(string[] pluginsToLocate);

        IEnumerable<LiveProject> GetAllProjects();
        
        void DeleteAll();
        
        int CountProjects();
    }
}
