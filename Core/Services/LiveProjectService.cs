
using System.Collections.Generic;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;

namespace AlsTools.Core.Services
{
    public class LiveProjectService : ILiveProjectService
    {
        private readonly ILiveProjectRepository repository;
        private readonly ILiveProjectFileSystem fs;
        private readonly ILiveProjectExtractor extractor;

        public LiveProjectService(ILiveProjectRepository repository, ILiveProjectFileSystem fs, ILiveProjectExtractor extractor)
        {
            this.repository = repository;
            this.fs = fs;
            this.extractor = extractor;
        }

        public IEnumerable<LiveProject> GetAllProjects()
        {
            return repository.GetAllProjects();
        }

        public IEnumerable<LiveProject> GetProjectsContainingPlugins(string[] pluginsToLocate)
        {
            return repository.GetProjectsContainingPlugins(pluginsToLocate);
        }

        public void InitializeDbFromFile(string filePath)
        {
            var project = LoadProjectFromSetFile(filePath);
            repository.Insert(project);
        }

        public void InitializeDbFromFolder(string folderPath, bool includeBackupFolder)
        {
            var projects = LoadProjectsFromDirectory(folderPath, includeBackupFolder);
            repository.Insert(projects);
        }

        private LiveProject LoadProjectFromSetFile(string setFilePath)
        {
            var file = fs.LoadProjectFileFromSetFile(setFilePath);            
            var project = extractor.ExtractProjectFromFile(file);
            
            return project;
        }

        private IList<LiveProject> LoadProjectsFromDirectory(string folderPath, bool includeBackupFolder)
        {
            List<LiveProject> projects = new List<LiveProject>();
            var files = fs.LoadProjectFilesFromDirectory(folderPath, includeBackupFolder);

            foreach (var f in files)
            {
                var project = extractor.ExtractProjectFromFile(f);
                projects.Add(project);
            }

            return projects;
        }


    }
}
