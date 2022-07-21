using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockRacks;

public abstract class BaseRackDeviceExtractor : BaseStockDeviceExtractor, IStockRackDeviceExtractor
{
    private readonly Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>> deviceTypeExtractors;
    private readonly IDictionary<string, DeviceType> deviceTypesByNodeDesc;

    public abstract string XPathDevicesSelector { get; }

    public BaseRackDeviceExtractor(ILogger<BaseRackDeviceExtractor> logger, Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>> deviceTypeExtractors, IDictionary<string, DeviceType> deviceTypesByNodeDesc) : base(logger)
    {
        this.deviceTypeExtractors = deviceTypeExtractors;
        this.deviceTypesByNodeDesc = deviceTypesByNodeDesc;
    }

    public override IDevice ExtractFromXml(XPathNavigator deviceNode)
    {
        var device = (BaseRackDevice)base.ExtractFromXml(deviceNode);
        var devicesFromBranches = GetDevicesFromCommonBranches(deviceNode);
        var devicesFromReturnBranches = GetDevicesFromReturnBranches(deviceNode);
        device.AddDevices(devicesFromBranches);
        device.AddDevices(devicesFromReturnBranches);

        return device;
    }

    protected IList<IDevice> GetDevicesFromCommonBranches(XPathNavigator nav)
    {
        logger.LogDebug("----");
        logger.LogDebug("Exctracting Rack device chains from XML...");

        var devices = new List<IDevice>();
        var devicesIterator = nav.Select(XPathDevicesSelector);

        // Iterate through all other devices
        while (devicesIterator.MoveNext())
        {
            if (devicesIterator.Current == null)
                continue;

            var deviceNode = devicesIterator.Current;
            var device = ExtractDeviceFromNode(deviceNode);
            devices.Add(device);

            if (device is BaseRackDevice rackDevice)
            {
                var childen = rackDevice.ChildrenDevices;
                devices.AddRange(childen.AsEnumerable());
            }
        }

        return devices;
    }

    protected virtual IList<IDevice> GetDevicesFromReturnBranches(XPathNavigator nav)
    {
        return BaseDevice.EmptyDevicesList;
    }

    protected IDevice ExtractDeviceFromNode(XPathNavigator deviceNode)
    {
        var type = GetDeviceTypeByDeviceNodeName(deviceNode.Name);
        var extractor = GetDeviceExtractorByDeviceType(type);

        return extractor.ExtractFromXml(deviceNode);
    }

    protected DeviceType GetDeviceTypeByDeviceNodeName(string deviceNodeName)
    {
        logger.LogDebug("Getting device type by device node name ({DeviceNodeName})...", deviceNodeName);

        var deviceNodeNameUpper = deviceNodeName.ToUpperInvariant();
        DeviceType type;

        if (deviceTypesByNodeDesc.TryGetValue(deviceNodeNameUpper, out type))
            return type;

        return DeviceType.Stock;
    }

    protected IDeviceTypeExtractor GetDeviceExtractorByDeviceType(DeviceType type)
    {
        logger.LogDebug("Getting device extractor by device type ({@DeviceType})...", type);

        var extractor = deviceTypeExtractors.Value[type];

        logger.LogDebug("Found device extractor: {@DeviceExtractor})", extractor);

        return extractor;
    }
}