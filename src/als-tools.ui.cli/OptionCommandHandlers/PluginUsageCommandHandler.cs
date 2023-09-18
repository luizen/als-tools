
namespace AlsTools.Ui.Cli;

public class PluginUsageCommandHandler : IOptionCommandHandler<PluginUsageOptions>
{
    private readonly ILogger<PluginUsageCommandHandler> logger;
    private readonly ILiveProjectAsyncService liveProjectService;
    private readonly IOptions<PlugInfoOptions> plugInfoOptions;

    public PluginUsageCommandHandler(ILogger<PluginUsageCommandHandler> logger, ILiveProjectAsyncService liveProjectService, IOptions<PlugInfoOptions> plugInfoOptions)
    {
        this.logger = logger;
        this.liveProjectService = liveProjectService;
        this.plugInfoOptions = plugInfoOptions;
    }

    public async Task Execute(PluginUsageOptions options)
    {
        logger.LogDebug("Listing {@UsedOrUnusedPluginsOnly} plugins only...", options.Select == PluginUsageSelection.UsedOnly ? "used" : "unused");

        var availablePlugins = GetAvailablePluginsFromPlugInfo();
        IReadOnlyList<PluginDevice> plugins = await liveProjectService.GetPluginUsageResults(availablePlugins, options.Select);

        var pluginNamesAndFormats = plugins.Select(plugin => new
        {
            PluginName = plugin.Name,
            PluginFormat = plugin.Format.ToString()
        });

        var json = JsonSerializer.Serialize<IEnumerable<dynamic>>(pluginNamesAndFormats, new JsonSerializerOptions { WriteIndented = true });
        await Console.Out.WriteLineAsync(json);
    }

    private IList<PluginDevice> GetAvailablePluginsFromPlugInfo()
    {
        List<string> vst2Plugins = File.ReadLines(plugInfoOptions.Value.InputFilePath)
            .Where(line => plugInfoOptions.Value.PluginPathOptions.Vst2Paths.Any(path => line.StartsWith(path)))
            .Select(line => Path.GetFileNameWithoutExtension(line))
            .ToList();

        List<string> vst3Plugins = File.ReadLines(plugInfoOptions.Value.InputFilePath)
            .Where(line => plugInfoOptions.Value.PluginPathOptions.Vst3Paths.Any(path => line.StartsWith(path)))
            .Select(line => Path.GetFileNameWithoutExtension(line))
            .ToList();

        List<string> auPlugins = File.ReadLines(plugInfoOptions.Value.InputFilePath)
            .Where(line => plugInfoOptions.Value.PluginPathOptions.AudioUnitPaths.Any(path => line.StartsWith(path)))
            .Select(line => Path.GetFileNameWithoutExtension(line))
            .ToList();

        IList<PluginDevice> plugins = new List<PluginDevice>();

        foreach (var pluginName in vst2Plugins)
        {
            plugins.Add(new PluginDevice(DeviceSort.Unknown, PluginFormat.VST2)
            {
                Name = pluginName
            });
        }

        foreach (var pluginName in vst3Plugins)
        {
            plugins.Add(new PluginDevice(DeviceSort.Unknown, PluginFormat.VST3)
            {
                Name = pluginName
            });
        }

        foreach (var pluginName in auPlugins)
        {
            plugins.Add(new PluginDevice(DeviceSort.Unknown, PluginFormat.AU)
            {
                Name = pluginName
            });
        }

        return plugins;
    }
}
