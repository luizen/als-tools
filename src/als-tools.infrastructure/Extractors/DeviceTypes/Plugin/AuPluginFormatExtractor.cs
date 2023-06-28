using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.Plugin;

public class AuPluginFormatExtractor : BasePluginFormatExtractor, IPluginFormatExtractor
{
    public AuPluginFormatExtractor(ILogger<AuPluginFormatExtractor> logger) : base(logger, PluginFormat.AU)
    {
    }

    protected override string PluginNameXpath => @"PluginDesc/AuPluginInfo/Name/@Value";

    protected override DeviceSort GetPluginSort(XPathNavigator pluginDescNode, string pluginName)
    {
        return DeviceSort.Unknown;
    }
}
