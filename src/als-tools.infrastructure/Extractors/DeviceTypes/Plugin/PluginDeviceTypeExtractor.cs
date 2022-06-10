using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects;
using AlsTools.Infrastructure.XmlNodeNames;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.Plugin;

public class PluginDeviceTypeExtractor : IDeviceExtractor
{
    private readonly ILogger<PluginDeviceTypeExtractor> logger;

    private readonly IDictionary<PluginFormat, IPluginFormatExtractor> pluginFormatExtractors;

    public PluginDeviceTypeExtractor(ILogger<PluginDeviceTypeExtractor> logger, IDictionary<PluginFormat, IPluginFormatExtractor> extractors)
    {
        if (extractors == null || extractors.Count == 0)
            throw new ArgumentNullException(nameof(extractors));

        this.logger = logger;
        this.pluginFormatExtractors = extractors;
    }

    public IDevice ExtractFromXml(XPathNavigator deviceNode)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting Plugin device from XML...");

        var formatExtractor = GetPluginFormatExtractor(deviceNode);

        var device = formatExtractor.ExtractFromXml(deviceNode);

        return device;
    }

    private IPluginFormatExtractor GetPluginFormatExtractor(XPathNavigator deviceNode)
    {
        var pluginDescNode = deviceNode.Select(@"PluginDesc");
        pluginDescNode.MoveNext();
        if (pluginDescNode.Current?.HasChildren ?? false)
        {
            if (pluginDescNode.Current.MoveToFirstChild())
            {
                var pluginInfoNodeName = pluginDescNode.Current.Name.ToUpperInvariant();
                var format = PluginFormatNodeName.GetPluginFormatFromNodeName(pluginInfoNodeName);

                return pluginFormatExtractors[format];
            }
        }

        throw new InvalidOperationException("Plugin format extractor not found");
    }
}