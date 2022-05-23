using System;
using System.Collections.Generic;
using System.Xml.XPath;
using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Infrastructure.Extractors.PluginTypes;
using Microsoft.Extensions.Logging;

namespace AlsTools.Infrastructure.Extractors;

public class PluginDeviceExtractor : IDeviceExtractor
{
    private readonly ILogger<PluginDeviceExtractor> logger;

    private readonly IDictionary<string, IPluginTypeExtractor> pluginTypeExtractors;

    public PluginDeviceExtractor(ILogger<PluginDeviceExtractor> logger, IDictionary<string, IPluginTypeExtractor> extractors)
    {
        if (extractors == null || extractors.Count == 0)
            throw new ArgumentNullException(nameof(extractors));

        this.logger = logger;
        this.pluginTypeExtractors = extractors;
    }

    public IDevice ExtractFromXml(XPathNavigator deviceNode)
    {
        logger.LogDebug("Extracting Plugin device from XML...");

        IDevice device = null;

        var pluginDescNode = deviceNode.Select(@"PluginDesc");
        pluginDescNode.MoveNext();
        if (pluginDescNode.Current.HasChildren)
        {
            if (pluginDescNode.Current.MoveToFirstChild())
            {
                device = GetPluginDevice(pluginDescNode.Current);
            }
        }

        return device;
    }

    private IDevice GetPluginDevice(XPathNavigator pluginDescNode)
    {
        var pluginDescNodeName = pluginDescNode.Name.ToUpperInvariant();
        logger.LogDebug("Extracting plugin details. Plugin description node name: {PluginDescNodeName}", pluginDescNodeName);

        var pluginDevice = pluginTypeExtractors[pluginDescNodeName].ExtractFromXml(pluginDescNode);
        return pluginDevice;
    }
}