namespace AlsTools.Core.Interfaces;

public interface ILiveProjectAsyncRepository
{
    Task InsertAsync(LiveProject project);

    Task InsertAsync(IEnumerable<LiveProject> projects);

    Task<IReadOnlyList<LiveProject>> Search(FilterContext filterContext);

    Task<IReadOnlyList<LiveProject>> GetAllProjectsAsync();

    Task DeleteAllAsync();

    Task<int> CountProjectsAsync();
}
