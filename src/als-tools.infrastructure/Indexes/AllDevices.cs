using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;


public class AllDevices : AbstractMultiMapIndexCreationTask<AllDevices.Result>
{
    public class Result : IEnabledResultSet
    {
        public string DeviceName { get; set; }
        public DeviceType Type { get; set; }
        public IDevice Device { get; set; }
        public PluginFormat? PluginFormat { get; set; } = null;  // for plugin only
        public bool IsEnabled { get; set; }
    }

    public AllDevices()
    {
        AddMap<LiveProject>(projects => from project in projects
                                        from track in project.Tracks
                                        from device in track.MaxForLiveDevices
                                        select new Result
                                        {
                                            DeviceName = device.Name,
                                            Type = DeviceType.MaxForLive,
                                            Device = device,
                                            PluginFormat = null, // needed, otherwise Index Compiler will complain
                                            IsEnabled = device.IsEnabled
                                        });

        AddMap<LiveProject>(projects => from project in projects
                                        from track in project.Tracks
                                        from device in track.Plugins
                                        select new Result
                                        {
                                            DeviceName = device.Name,
                                            Type = DeviceType.Plugin,
                                            Device = device,
                                            PluginFormat = device.Format, // for plugin only
                                            IsEnabled = device.IsEnabled
                                        });

        AddMap<LiveProject>(projects => from project in projects
                                        from track in project.Tracks
                                        from device in track.StockDevices
                                        select new Result
                                        {
                                            DeviceName = device.Name,
                                            Type = DeviceType.Stock,
                                            Device = device,
                                            PluginFormat = null, // needed, otherwise Index Compiler will complain
                                            IsEnabled = device.IsEnabled
                                        });

        Reduce = results => from result in results
                            group result by new { result.DeviceName, result.Type, result.PluginFormat } into g
                            select new Result
                            {
                                DeviceName = g.Key.DeviceName,
                                Type = g.Key.Type,
                                PluginFormat = g.Key.PluginFormat,
                                Device = g.First().Device,
                                IsEnabled = g.First().IsEnabled
                            };
    }
}
