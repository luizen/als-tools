using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;
using AlsTools.Infrastructure.Extractors;
using AlsTools.Infrastructure.XmlNodeNames;

namespace AlsTools.Infrastructure.Handlers;

public class DeviceExtractionHandler : IDeviceExtractionHandler
{
    private readonly ILogger<DeviceExtractionHandler> logger;
    private readonly IDictionary<DeviceType, IDeviceExtractor> deviceExtractors;

    private static readonly IDictionary<string, DeviceType> deviceTypesByNodeDesc = new Dictionary<string, DeviceType>()
    {
        [DeviceTypeNodeName.Plugin] = DeviceType.Plugin,
        [DeviceTypeNodeName.AuPlugin] = DeviceType.Plugin,
        [DeviceTypeNodeName.MaxForLiveAudioEffect] = DeviceType.MaxForLive,
        [DeviceTypeNodeName.MaxForLiveInstrument] = DeviceType.MaxForLive,
        [DeviceTypeNodeName.MaxForLiveMidiEffect] = DeviceType.MaxForLive
    };

    public DeviceExtractionHandler(ILogger<DeviceExtractionHandler> logger, IDictionary<DeviceType, IDeviceExtractor> deviceExtractors)
    {
        if (deviceExtractors == null || deviceExtractors.Count == 0)
            throw new ArgumentNullException(nameof(deviceExtractors));

        this.logger = logger;
        this.deviceExtractors = deviceExtractors;
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
                var deviceNode = devicesIterator.Current;
                var device = ExtractDeviceFromNode(deviceNode);
                devices.Add(device);

                if (device is BaseRackDevice rackDevice)
                {
                    var childen = rackDevice.ChildrenDevices;
                    devices.AddRange(childen.AsEnumerable());
                }

                // Iterate through all other devices
                while (devicesIterator.Current.MoveToNext())
                {
                    deviceNode = devicesIterator.Current;
                    device = ExtractDeviceFromNode(deviceNode);
                    devices.Add(device);

                    if (device is BaseRackDevice rackDevice2)
                    {
                        var childen = rackDevice2.ChildrenDevices;
                        devices.AddRange(childen.AsEnumerable());
                    }
                }
            }
        }

        return devices;
    }

    private IDevice ExtractDeviceFromNode(XPathNavigator deviceNode)
    {
        var type = GetDeviceTypeByDeviceNodeName(deviceNode.Name);
        var extractor = GetDeviceExtractorByDeviceType(type);

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

    private IDeviceExtractor GetDeviceExtractorByDeviceType(DeviceType type)
    {
        logger.LogDebug("Getting device extractor by device type ({@DeviceType})...", type);

        var extractor = deviceExtractors[type];

        logger.LogDebug("Found device extractor: {@DeviceExtractor})", extractor);

        return extractor;
    }
}