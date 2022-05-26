using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.PluginFormats;

public class AuPluginFormatExtractor : BasePluginFormatExtractor, IPluginFormatExtractor
{
    private readonly ILogger<AuPluginFormatExtractor> logger;

    public AuPluginFormatExtractor(ILogger<AuPluginFormatExtractor> logger) : base(logger, PluginFormat.AU)
    {
        this.logger = logger;
    }

    protected override string PluginNameXpath => @"PluginDesc/AuPluginInfo/Name/@Value";
    
    protected override DeviceSort GetPluginSort(XPathNavigator pluginDescNode, string pluginName)
    {
        return DeviceSort.Unknown;
    }
}