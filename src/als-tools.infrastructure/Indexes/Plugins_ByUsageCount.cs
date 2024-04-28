using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class Plugins_ByUsageCount : AbstractIndexCreationTask<LiveProject, DevicesUsageCountResult>
{
    public Plugins_ByUsageCount()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          from plugin in track.Plugins
                          select new DevicesUsageCountResult()
                          {
                              DeviceName = plugin.Name,
                              UsageCount = 1
                          };

        Reduce = results => from result in results
                            group result by result.DeviceName into g
                            select new DevicesUsageCountResult()
                            {
                                DeviceName = g.Key,
                                UsageCount = g.Sum(x => x.UsageCount)
                            };
    }
}