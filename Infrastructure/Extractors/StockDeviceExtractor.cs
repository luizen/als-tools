using System.Collections.Generic;
using System.Xml.XPath;
using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Infrastructure.Extractors.StockDevices;
using Microsoft.Extensions.Logging;

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
        logger.LogDebug("Extracting Live Stock device from XML...");

        var stockDeviceNodeName = deviceNode.Name.ToUpperInvariant();
        var device = stockDeviceExtractors[stockDeviceNodeName].ExtractFromXml(deviceNode);

        return device;
    }
}