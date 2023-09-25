using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.Plugin;

public class AuV3PluginFormatExtractor : BasePluginFormatExtractor, IPluginFormatExtractor
{
    public AuV3PluginFormatExtractor(ILogger<AuV3PluginFormatExtractor> logger) : base(logger, PluginFormat.AUv3)
    {
    }

    protected override string PluginNameXpath => @"PluginDesc/AuPluginInfo/Name/@Value";

    protected override DeviceSort GetPluginSort(XPathNavigator pluginDescNode, string pluginName)
    {
        return DeviceSort.Unknown;
    }
}
