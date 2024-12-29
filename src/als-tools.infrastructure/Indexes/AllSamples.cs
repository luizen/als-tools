using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class AllSamples : AbstractIndexCreationTask<LiveProject, StringItemResult>
{
    public AllSamples()
    {
        Map = projects => from project in projects
            from track in project.Tracks
            from sample in track.Samples
            where !string.IsNullOrEmpty(sample)
            select new StringItemResult { Value = sample };
        
        // Reduce = results => from result in results
        //     group result by new { result.DeviceName, result.Type, result.PluginFormat } into g
        //     select new Result
        //     {
        //         DeviceName = g.Key.DeviceName,
        //         Type = g.Key.Type,
        //         PluginFormat = g.Key.PluginFormat,
        //         Device = g.First().Device,
        //         IsEnabled = g.First().IsEnabled
        //     };
    }
}