using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class Plugins_ByUsageCount_EnabledOnly : AbstractIndexCreationTask<LiveProject, PluginsUsageCountResult>
{
    public Plugins_ByUsageCount_EnabledOnly()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          from plugin in track.Plugins
                          where plugin.IsEnabled
                          select new PluginsUsageCountResult()
                          {
                              PluginName = plugin.Name,
                              UsageCount = 1
                          };

        Reduce = results => from result in results
                            group result by result.PluginName into g
                            select new PluginsUsageCountResult()
                            {
                                PluginName = g.Key,
                                UsageCount = g.Sum(x => x.UsageCount)
                            };
    }
}