using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.Devices;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class AllStockDevices : AbstractIndexCreationTask<LiveProject, AllStockDevices.Result>
{
    public class Result
    {
        public required string StockDeviceName { get; set; }

        public required StockDevice StockDevice { get; set; }
    }

    public AllStockDevices()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          from stockDevice in track.StockDevices
                          select new Result()
                          {
                              StockDeviceName = stockDevice.Name,
                              StockDevice = stockDevice
                          };

        Reduce = results => from result in results
                            group result by result.StockDeviceName into g
                            select new Result
                            {
                                StockDeviceName = g.Key,
                                StockDevice = g.First().StockDevice
                            };
    }
}