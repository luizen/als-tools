using System.Linq.Expressions;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using AlsTools.Core.Queries;

namespace AlsTools.Core.Services;

public class LiveProjectAsyncService : ILiveProjectAsyncService
{
    private readonly ILogger<LiveProjectAsyncService> logger;
    private readonly ILiveProjectAsyncRepository repository;
    private readonly ILiveProjectFileSystem fs;
    private readonly ILiveProjectFileExtractionHandler extractor;

    public LiveProjectAsyncService(ILogger<LiveProjectAsyncService> logger, ILiveProjectAsyncRepository repository, ILiveProjectFileSystem fs, ILiveProjectFileExtractionHandler extractor)
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

    public async Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync()
    {
        return await repository.GetAllProjectsAsync();
    }

    public async Task<IReadOnlyList<LiveProject>> Search(QuerySpecification specification)
    {
        return await repository.Search(specification);
    }

    public async Task<IReadOnlyList<LiveProject>> Search(Expression<Func<LiveProject, bool>> filter)
    {
        return await repository.Search(filter);
    }

    public async Task<int> InitializeDbFromFilesAsync(IEnumerable<string> filePaths)
    {
        await repository.DeleteAllAsync();
        var project = LoadProjectsFromSetFiles(filePaths);
        await repository.InsertAsync(project);

        return 1;
    }

    public async Task<int> InitializeDbFromFoldersAsync(IEnumerable<string> folderPaths, bool includeBackupFolder)
    {
        await repository.DeleteAllAsync();
        var projects = LoadProjectsFromDirectories(folderPaths, includeBackupFolder);
        await repository.InsertAsync(projects);
        return projects.Count;
    }

    private IReadOnlyList<LiveProject> LoadProjectsFromSetFiles(IEnumerable<string> filePaths)
    {
        var files = fs.LoadProjectFilesFromSetFiles(filePaths);

        return ExtractProjectsFromFiles(files);
    }

    private IReadOnlyList<LiveProject> LoadProjectsFromDirectories(IEnumerable<string> folderPaths, bool includeBackupFolder)
    {
        var files = fs.LoadProjectFilesFromDirectories(folderPaths, includeBackupFolder);

        return ExtractProjectsFromFiles(files);
    }

    private IReadOnlyList<LiveProject> ExtractProjectsFromFiles(IEnumerable<FileInfo> files)
    {
        var projects = new List<LiveProject>();

        foreach (var f in files)
        {
            var project = extractor.ExtractProjectFromFile(f);
            projects.Add(project);
        }

        return projects;
    }


}
