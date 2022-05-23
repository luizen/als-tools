using System.Xml.XPath;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;
using Microsoft.Extensions.Logging;

namespace AlsTools.Infrastructure.Extractors.PluginTypes;

public class AuPluginTypeExtractor : IPluginTypeExtractor
{
    private readonly ILogger<AuPluginTypeExtractor> logger;

    public AuPluginTypeExtractor(ILogger<AuPluginTypeExtractor> logger)
    {
        this.logger = logger;
    }

    public IDevice ExtractFromXml(XPathNavigator pluginDescNode)
    {
        logger.LogDebug("Extracting AU plugin device...");

        var format = PluginFormat.AU;
        var sort = DeviceSort.Unknown;
        var pluginDevice = new PluginDevice(sort, format);
        pluginDevice.Name = pluginDescNode.SelectSingleNode(@"PluginDesc/AuPluginInfo/Name/@Value")?.Value;
        pluginDevice.UserName = pluginDescNode.SelectSingleNode(@"UserName/@Value")?.Value;
        pluginDevice.Annotation = pluginDescNode.SelectSingleNode(@"Annotation/@Value")?.Value;
        pluginDevice.Id = pluginDescNode.SelectSingleNode(@"@Id").ValueAsInt;

        return pluginDevice;
    }
}