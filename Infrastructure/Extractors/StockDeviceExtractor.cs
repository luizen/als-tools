using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Infrastructure.Extractors.StockDevices;

namespace AlsTools.Infrastructure.Extractors;

public class StockDeviceExtractor : IDeviceExtractor
{
    private readonly ILogger<StockDeviceExtractor> logger;
    private readonly IDictionary<string, IStockDeviceExtractor> stockDeviceExtractors;

    public StockDeviceExtractor(ILogger<StockDeviceExtractor> logger, IDictionary<string, IStockDeviceExtractor> extractors)
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
            return new UnknownLiveDevice(deviceNode.Name);            
        }

        var device = stockDeviceExtractors[stockDeviceNodeName].ExtractFromXml(deviceNode);
        return device;
    }
}