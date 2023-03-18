namespace AlsTools.Infrastructure.Extractors.DeviceTypes.MaxForLive;

public class MaxForLiveDeviceTypeExtractor : IDeviceTypeExtractor
{
    private readonly ILogger<MaxForLiveDeviceTypeExtractor> logger;

    private readonly IDictionary<string, IMaxForLiveDeviceSortExtractor> maxForLiveDeviceSortExtractors;

    public MaxForLiveDeviceTypeExtractor(ILogger<MaxForLiveDeviceTypeExtractor> logger, IDictionary<string, IMaxForLiveDeviceSortExtractor> maxForLiveDeviceSortExtractors)
    {
        this.logger = logger;
        this.maxForLiveDeviceSortExtractors = maxForLiveDeviceSortExtractors;
    }

    public IDevice ExtractFromXml(XPathNavigator deviceNode)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting MaxForLive device from XML...");

        var max4LiveDeviceDescNode = deviceNode.Name.ToUpperInvariant();
        var device = maxForLiveDeviceSortExtractors[max4LiveDeviceDescNode].ExtractFromXml(deviceNode);

        logger.LogDebug(@"Extracted MaxForLive device: {@DeviceName}", device.Name);

        return device;
    }
}