using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class MaxForLiveDevices_ByUsageCount : AbstractIndexCreationTask<LiveProject, DevicesUsageCountResult>
{
    public MaxForLiveDevices_ByUsageCount()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          from maxForLiveDevice in track.MaxForLiveDevices
                          select new DevicesUsageCountResult()
                          {
                              DeviceName = maxForLiveDevice.Name,
                              UsageCount = 1,
                              IsEnabled = maxForLiveDevice.IsEnabled
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