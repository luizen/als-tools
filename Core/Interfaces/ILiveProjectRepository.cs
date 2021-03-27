using System;
using System.Collections.Generic;
using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces
{
    public interface ILiveProjectRepository
    {
        void Insert(LiveProject project);

        void Insert(IList<LiveProject> projects);
        
        IList<LiveProject> GetProjectsContainingPlugins(string[] pluginsToLocate);

        IList<LiveProject> GetAllProjects();
    }
}
