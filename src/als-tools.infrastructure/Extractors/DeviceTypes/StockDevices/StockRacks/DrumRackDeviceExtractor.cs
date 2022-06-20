using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;
using AlsTools.Infrastructure.XmlNodeNames;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockRacks;

public class DrumRackDeviceExtractor : BaseStockDeviceExtractor, IStockRackDeviceExtractor
{
    private readonly Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>> deviceTypeExtractors;

    private static readonly IDictionary<string, DeviceType> deviceTypesByNodeDesc = new Dictionary<string, DeviceType>()
    {
        [DeviceTypeNodeName.Plugin] = DeviceType.Plugin,
        [DeviceTypeNodeName.AuPlugin] = DeviceType.Plugin,
        [DeviceTypeNodeName.MaxForLiveAudioEffect] = DeviceType.MaxForLive,
        [DeviceTypeNodeName.MaxForLiveInstrument] = DeviceType.MaxForLive,
        [DeviceTypeNodeName.MaxForLiveMidiEffect] = DeviceType.MaxForLive
    };

    public DrumRackDeviceExtractor(ILogger<DrumRackDeviceExtractor> logger, Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>> deviceTypeExtractors) : base(logger)
    {
        this.deviceTypeExtractors = deviceTypeExtractors;
    }

    protected override IDevice CreateDevice()
    {
        return new DrumRackDevice();
    }

    public override IDevice ExtractFromXml(XPathNavigator deviceNode)
    {
        var device = (DrumRackDevice)base.ExtractFromXml(deviceNode);

        var devicesFromDrumBranches = GetDevicesFromDrumBranches(deviceNode);
        var devicesFromReturnBranches = GetDevicesFromReturnBranches(deviceNode);

        device.AddDevices(devicesFromDrumBranches);
        device.AddDevices(devicesFromReturnBranches);

        return device;
    }

    private IList<IDevice> GetDevicesFromDrumBranches(XPathNavigator nav)
    {
        logger.LogDebug("----");
        logger.LogDebug("Exctracting devices of DrumRack drum chains from XML...");

        var devices = new List<IDevice>();
        var devicesIterator = nav.Select(@".//Branches/DrumBranch/DeviceChain/MidiToAudioDeviceChain/Devices/*");

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

    private IList<IDevice> GetDevicesFromReturnBranches(XPathNavigator nav)
    {
        logger.LogDebug("----");
        logger.LogDebug("Exctracting devices of DrumRack return chains from XML...");

        var devices = new List<IDevice>();
        var devicesIterator = nav.Select(@".//ReturnBranches/ReturnBranch/DeviceChain/AudioToAudioDeviceChain/Devices/*");

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

    private IDeviceTypeExtractor GetDeviceExtractorByDeviceType(DeviceType type)
    {
        logger.LogDebug("Getting device extractor by device type ({@DeviceType})...", type);

        var extractor = deviceTypeExtractors.Value[type];

        logger.LogDebug("Found device extractor: {@DeviceExtractor})", extractor);

        return extractor;
    }
}