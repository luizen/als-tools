using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;


public class LiveProjects_PluginsCount : AbstractIndexCreationTask<LiveProject, ItemsCountPerProjectResult>
{
    public LiveProjects_PluginsCount()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          from plugin in track.Plugins
                          select new ItemsCountPerProjectResult
                          {
                              ProjectId = project.Id,
                              ProjectName = project.Name,
                              ItemsCount = 1,
                              IsEnabled = plugin.IsEnabled
                          };

        Reduce = results => from result in results
                            group result by new { result.ProjectId, result.ProjectName, result.IsEnabled } into g
                            select new ItemsCountPerProjectResult
                            {
                                ProjectId = g.Key.ProjectId,
                                ProjectName = g.Key.ProjectName,
                                ItemsCount = g.Sum(x => x.ItemsCount),
                                IsEnabled = g.Key.IsEnabled
                            };
    }
}