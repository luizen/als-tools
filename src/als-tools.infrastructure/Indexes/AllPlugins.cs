using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.Devices;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class AllPlugins : AbstractIndexCreationTask<LiveProject, AllPlugins.Result>
{
    public class Result
    {
        public required string PluginName { get; set; }

        public required PluginDevice Plugin { get; set; }
    }

    public AllPlugins()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          from plugin in track.Plugins
                          select new Result()
                          {
                              PluginName = plugin.Name,
                              Plugin = plugin
                          };

        Reduce = results => from result in results
                            group result by result.PluginName into g
                            select new Result
                            {
                                PluginName = g.Key,
                                Plugin = g.First().Plugin
                            };
    }
}