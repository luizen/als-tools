using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class LiveProjects_TracksCount : AbstractIndexCreationTask<LiveProject, TracksCountPerProjectResult>
{
    public LiveProjects_TracksCount()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          select new TracksCountPerProjectResult()
                          {
                              ProjectName = project.Name,
                              ProjectPath = project.Path,
                              TracksCount = 1
                          };

        Reduce = results => from result in results
                            group result by new { result.ProjectPath, result.ProjectName } into g
                            select new TracksCountPerProjectResult()
                            {
                                ProjectName = g.Key.ProjectName,
                                ProjectPath = g.Key.ProjectPath,
                                TracksCount = g.Sum(x => x.TracksCount)
                            };
    }
}
