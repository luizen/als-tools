namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices;

public class StockDeviceDeviceTypeExtractor : IDeviceTypeExtractor
{
    private readonly ILogger<StockDeviceDeviceTypeExtractor> logger;

    private readonly IDictionary<string, IStockDeviceExtractor> stockDeviceExtractors;

    public StockDeviceDeviceTypeExtractor(ILogger<StockDeviceDeviceTypeExtractor> logger, IDictionary<string, IStockDeviceExtractor> extractors)
    {
        this.logger = logger;
        this.stockDeviceExtractors = extractors;
    }

    public IDevice ExtractFromXml(XPathNavigator deviceNode)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting Live Stock device from XML...");

        var stockDeviceNodeName = deviceNode.Name.ToUpperInvariant();

        if (!stockDeviceExtractors.ContainsKey(stockDeviceNodeName))
        {
            logger.LogWarning(@"A stock device with node named '{@DeviceNodeName}' does not have a valid extractor for it.", deviceNode.Name);
            return new UnknownStockDevice(deviceNode.Name);
        }

        var extractor = stockDeviceExtractors[stockDeviceNodeName];
        var device = extractor.ExtractFromXml(deviceNode);
        return device;
    }
}