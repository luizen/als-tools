using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class LiveProjects_PluginsCount_EnabledOnly : AbstractIndexCreationTask<LiveProject, PluginsCountPerProjectResult>
{
    public LiveProjects_PluginsCount_EnabledOnly()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          from plugin in track.Plugins
                          where plugin.IsEnabled
                          select new PluginsCountPerProjectResult()
                          {
                              ProjectName = project.Name,
                              ProjectPath = project.Path,
                              PluginsCount = 1
                          };

        Reduce = results => from result in results
                            group result by new { result.ProjectPath, result.ProjectName } into g
                            select new PluginsCountPerProjectResult()
                            {
                                ProjectName = g.Key.ProjectName,
                                ProjectPath = g.Key.ProjectPath,
                                PluginsCount = g.Sum(x => x.PluginsCount)
                            };
    }
}
