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
        logger.LogDebug("----");
        logger.LogDebug("Extracting {@PluginFormat} plugin device...", pluginFormat);
        var pluginName = pluginDeviceNode.SelectSingleNode(PluginNameXpath)!.Value;
        logger.LogDebug("Plugin found: {@PluginName} ", pluginName);

        var sort = GetPluginSort(pluginDeviceNode, pluginName);
        var pluginDevice = new PluginDevice(sort, pluginFormat);
        pluginDevice.Name = pluginName;
        pluginDevice.UserName = pluginDeviceNode.SelectSingleNode(@"UserName/@Value")!.Value;
        pluginDevice.Annotation = pluginDeviceNode.SelectSingleNode(@"Annotation/@Value")!.Value;
        pluginDevice.Id = pluginDeviceNode.SelectSingleNode(@"@Id")!.ValueAsInt;

        return pluginDevice;
    }

    protected abstract string PluginNameXpath { get; }

    protected abstract DeviceSort GetPluginSort(XPathNavigator pluginDescNode, string pluginName);
}