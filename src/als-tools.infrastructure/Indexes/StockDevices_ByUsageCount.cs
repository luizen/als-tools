using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class StockDevices_ByUsageCount : AbstractIndexCreationTask<LiveProject, StockDevicesUsageCountResult>
{
    public StockDevices_ByUsageCount()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          from stockDevice in track.StockDevices
                          select new StockDevicesUsageCountResult()
                          {
                              StockDeviceName = stockDevice.Name,
                              UsageCount = 1
                          };

        Reduce = results => from result in results
                            group result by result.StockDeviceName into g
                            select new StockDevicesUsageCountResult()
                            {
                                StockDeviceName = g.Key,
                                UsageCount = g.Sum(x => x.UsageCount)
                            };
    }
}