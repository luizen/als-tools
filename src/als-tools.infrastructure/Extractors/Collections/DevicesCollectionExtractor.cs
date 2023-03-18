namespace AlsTools.Infrastructure.Extractors.Collections;

public interface IDevicesCollectionExtractor : ICollectionExtractor<IDevice>
{
}

public class DevicesCollectionExtractor : IDevicesCollectionExtractor
{
    private readonly ILogger<DevicesCollectionExtractor> logger;
    private readonly Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>> deviceTypeExtractors;

    private static readonly IDictionary<string, DeviceType> deviceTypesByNodeDesc = new Dictionary<string, DeviceType>()
    {
        [DeviceTypeNodeName.Plugin] = DeviceType.Plugin,
        [DeviceTypeNodeName.AuPlugin] = DeviceType.Plugin,
        [DeviceTypeNodeName.MaxForLiveAudioEffect] = DeviceType.MaxForLive,
        [DeviceTypeNodeName.MaxForLiveInstrument] = DeviceType.MaxForLive,
        [DeviceTypeNodeName.MaxForLiveMidiEffect] = DeviceType.MaxForLive
    };

    public DevicesCollectionExtractor(ILogger<DevicesCollectionExtractor> logger, Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>> deviceTypeExtractors)
    {
        if (deviceTypeExtractors == null || deviceTypeExtractors.Value.Count == 0)
            throw new ArgumentNullException(nameof(deviceTypeExtractors));

        this.logger = logger;
        this.deviceTypeExtractors = deviceTypeExtractors;
    }

    public IReadOnlyList<IDevice> ExtractFromXml(XPathNavigator nav)
    {
        logger.LogDebug("----");
        logger.LogDebug("Exctracting devices from XML...");

        var devices = new List<IDevice>();
        var devicesIterator = nav.Select(@"DeviceChain/DeviceChain/Devices");
        devicesIterator.MoveNext();

        if (devicesIterator.Current?.HasChildren ?? false)
        {
            if (devicesIterator.Current.MoveToFirstChild())
            {
                // Get first device
                ExtractDeviceIntoDevicesList(devices, devicesIterator.Current);

                // Iterate through all other devices
                while (devicesIterator.Current.MoveToNext())
                {
                    ExtractDeviceIntoDevicesList(devices, devicesIterator.Current);
                }
            }
        }

        return devices;
    }

    private void ExtractDeviceIntoDevicesList(List<IDevice> devices, XPathNavigator deviceNode)
    {
        var device = ExtractDeviceFromNode(deviceNode);
        devices.Add(device);

        if (device is BaseRackDevice rackDevice)
        {
            var childen = rackDevice.ChildrenDevices;
            devices.AddRange(childen.AsEnumerable());
        }
    }

    private IDevice ExtractDeviceFromNode(XPathNavigator deviceNode)
    {
        var type = GetDeviceTypeByDeviceNodeName(deviceNode.Name);
        var extractor = GetDeviceTypeExtractorByDeviceType(type);

        return extractor.ExtractFromXml(deviceNode);
    }

    private DeviceType GetDeviceTypeByDeviceNodeName(string deviceNodeName)
    {
        logger.LogDebug("Getting device type by device node name ({DeviceNodeName})...", deviceNodeName);

        var deviceNodeNameUpper = deviceNodeName.ToUpperInvariant();
        DeviceType type;

        if (deviceTypesByNodeDesc.TryGetValue(deviceNodeNameUpper, out type))
            return type;

        return DeviceType.Stock;
    }

    private IDeviceTypeExtractor GetDeviceTypeExtractorByDeviceType(DeviceType type)
    {
        logger.LogDebug("Getting device extractor by device type ({@DeviceType})...", type);

        var extractor = deviceTypeExtractors.Value[type];

        logger.LogDebug("Found device extractor: {@DeviceExtractor})", extractor);

        return extractor;
    }
}