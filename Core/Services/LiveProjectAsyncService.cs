
using System.Collections.Generic;
using System.Threading.Tasks;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AlsTools.Core.Services
{
    public class LiveProjectAsyncService : ILiveProjectAsyncService
    {
        private readonly ILogger<LiveProjectAsyncService> logger;
        private readonly ILiveProjectAsyncRepository repository;
        private readonly ILiveProjectFileSystem fs;
        private readonly ILiveProjectExtractor extractor;

        public LiveProjectAsyncService(ILogger<LiveProjectAsyncService> logger, ILiveProjectAsyncRepository repository, ILiveProjectFileSystem fs, ILiveProjectExtractor extractor)
        {
            this.logger = logger;
            this.repository = repository;
            this.fs = fs;
            this.extractor = extractor;
        }

        public async Task<int> CountProjectsAsync()
        {
            return await repository.CountProjectsAsync();
        }

        public async Task<IEnumerable<LiveProject>> GetAllProjectsAsync()
        {
            return await repository.GetAllProjectsAsync();
        }

        public async Task<IEnumerable<LiveProject>> GetProjectsContainingPluginsAsync(string[] pluginsToLocate)
        {
            return await repository.GetProjectsContainingPluginsAsync(pluginsToLocate);
        }

        public async Task<int> InitializeDbFromFileAsync(string filePath)
        {
            await repository.DeleteAllAsync();
            var project = LoadProjectFromSetFile(filePath);
            await repository.InsertAsync(project);

            return 1;
        }

        public async Task<int> InitializeDbFromFolderAsync(string folderPath, bool includeBackupFolder)
        {
            await repository.DeleteAllAsync();
            var projects = LoadProjectsFromDirectory(folderPath, includeBackupFolder);
            await repository.InsertAsync(projects);
            return projects.Count;
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
