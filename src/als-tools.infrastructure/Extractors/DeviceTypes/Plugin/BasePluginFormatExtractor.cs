using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.Plugin;

public abstract class BasePluginFormatExtractor : IPluginFormatExtractor
{
    protected readonly ILogger<BasePluginFormatExtractor> logger;

    private readonly PluginFormat pluginFormat;

    public BasePluginFormatExtractor(ILogger<BasePluginFormatExtractor> logger, PluginFormat pluginFormat)
    {
        this.logger = logger;
        this.pluginFormat = pluginFormat;
    }

    public virtual IDevice ExtractFromXml(XPathNavigator pluginDeviceNode)
    {
        string pluginPath = string.Empty;

        logger.LogDebug("----");
        logger.LogDebug("Extracting {@PluginFormat} plugin device...", pluginFormat);
        var pluginName = pluginDeviceNode.SelectSingleNode(PluginNameXpath)!.Value;
        logger.LogDebug("Plugin found: {@PluginName} ", pluginName);

        if (!string.IsNullOrWhiteSpace(PluginPathXpath))
        {
            var node = pluginDeviceNode.SelectSingleNode(PluginPathXpath);
            pluginPath = node?.Value ?? string.Empty;
        }

        var sort = GetPluginSort(pluginDeviceNode, pluginName);
        var pluginDevice = new PluginDevice(sort, pluginFormat, pluginPath)
        {
            Name = pluginName,
            UserName = pluginDeviceNode.SelectSingleNode(@"UserName/@Value")!.Value,
            Annotation = pluginDeviceNode.SelectSingleNode(@"Annotation/@Value")!.Value,
            Id = pluginDeviceNode.SelectSingleNode(@"@Id")!.ValueAsInt,
            IsOn = pluginDeviceNode.SelectSingleNode(@"On/Manual/@Value")?.ValueAsBoolean
        };

        return pluginDevice;
    }

    protected abstract string PluginNameXpath { get; }

    protected virtual string PluginPathXpath 
    { 
        get
        {
            return string.Empty;
        }
    }

    protected abstract DeviceSort GetPluginSort(XPathNavigator pluginDescNode, string pluginName);
}