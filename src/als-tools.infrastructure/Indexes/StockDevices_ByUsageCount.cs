using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class StockDevices_ByUsageCount : AbstractIndexCreationTask<LiveProject, DevicesUsageCountResult>
{
    public StockDevices_ByUsageCount()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          from stockDevice in track.StockDevices
                          select new DevicesUsageCountResult()
                          {
                              DeviceName = stockDevice.Name,
                              UsageCount = 1,
                              IsEnabled = stockDevice.IsEnabled
                          };

        Reduce = results => from result in results
                            group result by result.DeviceName into g
                            select new DevicesUsageCountResult()
                            {
                                DeviceName = g.Key,
                                UsageCount = g.Sum(x => x.UsageCount),
                                IsEnabled = g.First().IsEnabled
                            };
    }
}