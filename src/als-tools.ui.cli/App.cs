using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
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


        logger.LogDebug("Deleting DB...");
        await liveProjectService.DeleteAllAsync();


        logger.LogDebug("Getting all projects...");
        var projects = await liveProjectService.GetAllProjectsAsync();
        Debug.Assert(projects.Count == 0, "Projects should be empty");


        logger.LogDebug("Inserting a project...");
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
        var countInserted = await liveProjectService.InsertAsync(project);
        Debug.Assert(countInserted == 7, "Inserted count should be 1");

        logger.LogDebug("Getting all projects again...");
        var projects2 = await liveProjectService.GetAllProjectsAsync();
        Debug.Assert(projects2.Count == 1, "Projects count should be 1 again...");

        logger.LogDebug("Comparing inserted and retrieved projcts...");
        var loadedProject = projects2.Single();

        logger.LogDebug("Original project: ");

        JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        var fullJsonData = JsonSerializer.Serialize(project, options);
        await Console.Out.WriteLineAsync(fullJsonData);

        logger.LogDebug("Loaded project:");
        var fullJsonData2 = JsonSerializer.Serialize(loadedProject, options);
        await Console.Out.WriteLineAsync(fullJsonData2);


        await Task.Delay(1);
    }
}