using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.Plugin;

public class AuV2PluginFormatExtractor : BasePluginFormatExtractor, IPluginFormatExtractor
{
    public AuV2PluginFormatExtractor(ILogger<AuV2PluginFormatExtractor> logger) : base(logger, PluginFormat.AUv2)
    {
    }

    protected override string PluginNameXpath => @"PluginDesc/AuPluginInfo/Name/@Value";

    protected override DeviceSort GetPluginSort(XPathNavigator pluginDescNode, string pluginName)
    {
        return DeviceSort.Unknown;
    }
}
