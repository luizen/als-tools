using AlsTools.Core.Models;

namespace AlsTools.Ui.Cli;

public class App
{
    private readonly ILogger<App> logger;
    private readonly ILiveProjectAsyncService liveProjectService;
    public App(ILogger<App> logger, ILiveProjectAsyncService liveProjectService)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
    }

    public async Task Run()
    {
        logger.LogDebug("Starting application...");

        var projects = await liveProjectService.GetAllProjectsAsync();

        var project = new Project
        {
            Name = "Test Project",
            Tracks = new List<Track>
            {
                new Track
                {
                    Name = "Test Track 1",
                    PluginDevices = new List<MyPluginDevice>
                    {
                        new MyPluginDevice
                        {
                            Name = "ADPTR Audio - Streamliner",
                            Format = Core.ValueObjects.PluginFormat.AUv3,
                            Sort = Core.ValueObjects.Devices.DeviceSort.MidiInstrument,
                            Type = Core.ValueObjects.Devices.DeviceType.Plugin
                        }
                    },
                    StockDevices = new List<MyStockDevice>
                    {
                        new MyStockDevice
                        {
                            Name = "Impulse",
                            Sort = Core.ValueObjects.Devices.DeviceSort.MidiInstrument,
                            Type = Core.ValueObjects.Devices.DeviceType.Stock
                        }
                    }
                },
                new Track
                {
                    Name = "Test Track2",
                    PluginDevices = new List<MyPluginDevice>
                    {
                        new MyPluginDevice
                        {
                            Name = "BlackBox HG-2MS",
                            Format = Core.ValueObjects.PluginFormat.VST3,
                            Sort = Core.ValueObjects.Devices.DeviceSort.AudioEffect,
                            Type = Core.ValueObjects.Devices.DeviceType.Plugin
                        }
                    },
                    StockDevices = new List<MyStockDevice>
                    {
                        new MyStockDevice
                        {
                            Name = "Amp",
                            Sort = Core.ValueObjects.Devices.DeviceSort.AudioEffect,
                            Type = Core.ValueObjects.Devices.DeviceType.Stock
                        }
                    }
                }
            },
            Path = "~/Desktop",
            Tempo = 138
        };

        await liveProjectService.InsertAsync(project);



        await Task.Delay(1);
    }
}