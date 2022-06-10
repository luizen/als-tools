using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.MaxForLive;

public class MaxForLiveDeviceTypeExtractor : IDeviceExtractor
{
    private readonly ILogger<MaxForLiveDeviceTypeExtractor> logger;

    private readonly IDictionary<string, IMaxForLiveDeviceSortExtractor> maxForLiveExtractors;

    public MaxForLiveDeviceTypeExtractor(ILogger<MaxForLiveDeviceTypeExtractor> logger, IDictionary<string, IMaxForLiveDeviceSortExtractor> extractors)
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