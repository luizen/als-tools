using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Infrastructure.XmlNodeNames;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices;

public abstract class BaseStockDeviceExtractor : IStockDeviceExtractor
{
    protected readonly ILogger<BaseStockDeviceExtractor> logger;

    protected abstract IDevice CreateDevice();

    public BaseStockDeviceExtractor(ILogger<BaseStockDeviceExtractor> logger)
    {
        this.logger = logger;
    }

    public virtual IDevice ExtractFromXml(XPathNavigator deviceNode)
    {
        var device = CreateDevice();

        logger.LogDebug("----");
        logger.LogDebug("Extracting Live Stock {DeviceSort} device from XML...", device.Family.Sort);

        var readableName = LiveStockDeviceNodeNames.GetDeviceNameByNodeName(deviceNode.Name);

        device.Name = readableName;

        logger.LogDebug("Device name: {DeviceName}", device.Name);

        device.UserName = deviceNode.SelectSingleNode(@"UserName/@Value")!.Value;
        device.Annotation = deviceNode.SelectSingleNode(@"Annotation/@Value")!.Value;
        device.Id = deviceNode.SelectSingleNode(@"@Id")!.ValueAsInt;
        device.IsOn = deviceNode.SelectSingleNode(@"On/Manual/@Value")?.ValueAsBoolean;

        return device;
    }
}