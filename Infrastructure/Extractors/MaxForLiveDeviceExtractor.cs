using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Infrastructure.Extractors.MaxForLiveSorts;

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
        logger.LogDebug("----");
        logger.LogDebug("Extracting MaxForLive device from XML...");

        var max4LiveDeviceDescNode = deviceNode.Name.ToUpperInvariant();
        var device = maxForLiveExtractors[max4LiveDeviceDescNode].ExtractFromXml(deviceNode);

        logger.LogDebug(@"Extracted MaxForLive device: {@DeviceName}", device.Name);

        return device;
    }
}