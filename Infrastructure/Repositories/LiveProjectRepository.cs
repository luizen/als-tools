using System.Collections.Generic;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;

namespace AlsTools.Infrastructure.Repositories
{
    public class LiveProjectRepository : ILiveProjectRepository
    {
        private static List<LiveProject> fakeProjects = new List<LiveProject>();

        public IList<LiveProject> GetAllProjects()
        {
            return fakeProjects;
        }

        public IList<LiveProject> GetProjectsContainingPlugins(string[] pluginsToLocate)
        {
            return fakeProjects;
        }

        public void Insert(LiveProject project)
        {
            fakeProjects.Add(project);
        }

        public void Insert(IList<LiveProject> projects)
        {
            fakeProjects.AddRange(projects);
        }
    }
}
