
using System.Collections.Generic;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AlsTools.Core.Services
{
    public class LiveProjectService : ILiveProjectService
    {
        private readonly ILogger<LiveProjectService> logger;
        private readonly ILiveProjectRepository repository;
        private readonly ILiveProjectFileSystem fs;
        private readonly ILiveProjectExtractor extractor;

        public LiveProjectService(ILogger<LiveProjectService> logger, ILiveProjectRepository repository, ILiveProjectFileSystem fs, ILiveProjectExtractor extractor)
        {
            this.logger = logger;
            this.repository = repository;
            this.fs = fs;
            this.extractor = extractor;
        }

        public int CountProjects()
        {
            return repository.CountProjects();
        }

        public IEnumerable<LiveProject> GetAllProjects()
        {
            return repository.GetAllProjects();
        }

        public IEnumerable<LiveProject> GetProjectsContainingPlugins(string[] pluginsToLocate)
        {
            return repository.GetProjectsContainingPlugins(pluginsToLocate);
        }

        public int InitializeDbFromFile(string filePath)
        {            
            repository.DeleteAll();
            var project = LoadProjectFromSetFile(filePath);
            return repository.Insert(project) ? 1 : 0;
        }

        public int InitializeDbFromFolder(string folderPath, bool includeBackupFolder)
        {
            repository.DeleteAll();
            var projects = LoadProjectsFromDirectory(folderPath, includeBackupFolder);
            return repository.Insert(projects);
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
