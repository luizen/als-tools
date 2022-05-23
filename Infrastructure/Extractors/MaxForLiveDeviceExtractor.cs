using System.Collections.Generic;
using System.Xml.XPath;
using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Infrastructure.Extractors.MaxForLiveSorts;
using Microsoft.Extensions.Logging;

namespace AlsTools.Infrastructure.Extractors;

public class MaxForLiveDeviceExtractor : IDeviceExtractor
{
    private readonly ILogger<MaxForLiveDeviceExtractor> logger;

    private readonly IDictionary<string, IMaxForLiveSortExtractor> maxForLiveExtractors;

    public MaxForLiveDeviceExtractor(ILogger<MaxForLiveDeviceExtractor> logger, IDictionary<string, IMaxForLiveSortExtractor> extractors)
    {
        this.logger = logger;
        this.maxForLiveExtractors = extractors;
    }

    public IDevice ExtractFromXml(XPathNavigator deviceNode)
    {
        logger.LogDebug("Extracting MaxForLive device from XML...");

        var max4LiveDeviceDescNode = deviceNode.Name.ToUpperInvariant();
        var device = maxForLiveExtractors[max4LiveDeviceDescNode].ExtractFromXml(deviceNode);

        return device;
    }
}