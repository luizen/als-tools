using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;


namespace AlsTools.Infrastructure.Indexes;

public class LiveProject_WithChildrenCounts : AbstractIndexCreationTask<LiveProject, LiveProjectWithChildrenCountsResult>
{
    public LiveProject_WithChildrenCounts()
    {
        Map = projects => from proj in projects
                          select new LiveProjectWithChildrenCountsResult()
                          {
                              Project = proj,
                              TracksCount = proj.Tracks.Count,
                              StockDevicesCount = proj.Tracks.Sum(track => track.StockDevices.Count),
                              PluginsCount = proj.Tracks.Sum(track => track.Plugins.Count),
                              MaxForLiveDevicesCount = proj.Tracks.Sum(track => track.MaxForLiveDevices.Count),
                              ScenesCount = proj.Scenes.Count,
                              LocatorsCount = proj.Locators.Count
                          };


        Reduce = results => from result in results
                            group result by result.Project into g
                            select new LiveProjectWithChildrenCountsResult
                            {
                                Project = g.Key,
                                TracksCount = g.Sum(x => x.TracksCount),
                                StockDevicesCount = g.Sum(x => x.StockDevicesCount),
                                PluginsCount = g.Sum(x => x.PluginsCount),
                                MaxForLiveDevicesCount = g.Sum(x => x.MaxForLiveDevicesCount),
                                ScenesCount = g.Sum(x => x.ScenesCount),
                                LocatorsCount = g.Sum(x => x.LocatorsCount)
                            };
    }
}
