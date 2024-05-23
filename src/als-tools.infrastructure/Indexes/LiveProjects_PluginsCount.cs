using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

// public class LiveProjects_PluginsCount : AbstractIndexCreationTask<LiveProject, ItemsCountPerProjectResult>
// {
//     public LiveProjects_PluginsCount()
//     {
//         Map = projects => from project in projects
//                           from track in project.Tracks
//                           from plugin in track.Plugins
//                           select new ItemsCountPerProjectResult()
//                           {
//                               ProjectName = project.Name,
//                               ProjectPath = project.Path,
//                               ItemsCount = 1
//                           };

//         Reduce = results => from result in results
//                             group result by new { result.ProjectPath, result.ProjectName } into g
//                             select new ItemsCountPerProjectResult()
//                             {
//                                 ProjectName = g.Key.ProjectName,
//                                 ProjectPath = g.Key.ProjectPath,
//                                 ItemsCount = g.Sum(x => x.ItemsCount)
//                             };
//     }
// }


public class LiveProjects_PluginsCount : AbstractIndexCreationTask<LiveProject, ItemsCountPerProjectResult>
{
    // public class Result
    // {
    //     public string ProjectId { get; set; }
    //     public int Count { get; set; }
    //     public bool IsEnabled { get; set; }
    // }

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