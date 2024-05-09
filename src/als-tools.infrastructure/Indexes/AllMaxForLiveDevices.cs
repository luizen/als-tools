using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.Devices;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class AllMaxForLiveDevices : AbstractIndexCreationTask<LiveProject, AllMaxForLiveDevices.Result>
{
    public class Result
    {
        public required string MaxForLiveDeviceName { get; set; }

        public required MaxForLiveDevice MaxForLiveDevice { get; set; }
    }

    public AllMaxForLiveDevices()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          from maxForLiveDevice in track.MaxForLiveDevices
                          select new Result()
                          {
                              MaxForLiveDeviceName = maxForLiveDevice.Name,
                              MaxForLiveDevice = maxForLiveDevice
                          };

        Reduce = results => from result in results
                            group result by result.MaxForLiveDeviceName into g
                            select new Result
                            {
                                MaxForLiveDeviceName = g.Key,
                                MaxForLiveDevice = g.First().MaxForLiveDevice
                            };
    }
}