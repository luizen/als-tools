using AlsTools.Core;
using AlsTools.Core.Entities;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure;

public class LiveProjects_TrackCount : AbstractIndexCreationTask<LiveProject, TracksCountPerProjectResult>
{
    public LiveProjects_TrackCount()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          select new
                          {
                              ProjectName = project.Name,
                              ProjectPath = project.Path,
                              TrackCount = 1
                          };

        Reduce = results => from result in results
                            group result by new { result.ProjectPath, result.ProjectName } into g
                            select new
                            {
                                Name = g.Key.ProjectName,
                                Path = g.Key.ProjectPath,
                                Count = g.Sum(x => x.TrackCount)
                            };
    }
}
