using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Infrastructure.XmlNodeNames;

namespace AlsTools.Infrastructure.Extractors.StockDevices;

public abstract class BaseStockDeviceExtractor : IStockDeviceExtractor
{
    private readonly ILogger<BaseStockDeviceExtractor> logger;

    public DeviceSort DeviceSort { get; protected set; }

    public BaseStockDeviceExtractor(ILogger<BaseStockDeviceExtractor> logger, DeviceSort sort)
    {
        this.logger = logger;
        this.DeviceSort = sort;
    }

    public virtual IDevice ExtractFromXml(XPathNavigator deviceNode)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting Live Stock {DeviceSort} device from XML...", DeviceSort);

        var device = new LiveDevice(DeviceSort);

        var readableName = LiveStockDeviceNodeNames.GetDeviceNameByNodeName(deviceNode.Name);

        device.Name = readableName;

        logger.LogDebug("Device name: {DeviceName}", device.Name);

        device.UserName = deviceNode.SelectSingleNode(@"UserName/@Value")?.Value;
        device.Annotation = deviceNode.SelectSingleNode(@"Annotation/@Value")?.Value;
        device.Id = deviceNode.SelectSingleNode(@"@Id").ValueAsInt;

        return device;
    }
}