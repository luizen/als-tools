using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class LiveProjects_StockDevicesCount_EnabledOnly : AbstractIndexCreationTask<LiveProject, StockDevicesCountPerProjectResult>
{
    public LiveProjects_StockDevicesCount_EnabledOnly()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          from stockDevice in track.StockDevices
                          where stockDevice.IsEnabled
                          select new StockDevicesCountPerProjectResult()
                          {
                              ProjectName = project.Name,
                              ProjectPath = project.Path,
                              StockDevicesCount = 1
                          };

        Reduce = results => from result in results
                            group result by new { result.ProjectPath, result.ProjectName } into g
                            select new StockDevicesCountPerProjectResult()
                            {
                                ProjectName = g.Key.ProjectName,
                                ProjectPath = g.Key.ProjectPath,
                                StockDevicesCount = g.Sum(x => x.StockDevicesCount)
                            };
    }
}
