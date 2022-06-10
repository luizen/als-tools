using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;
using AlsTools.Infrastructure.XmlNodeNames;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockRacks;

public class DrumRackDeviceExtractor : BaseStockDeviceExtractor, IStockRackDeviceExtractor
{
    // private readonly IDictionary<DeviceType, IDeviceExtractor> deviceExtractors;

    private static readonly IDictionary<string, DeviceType> deviceTypesByNodeDesc = new Dictionary<string, DeviceType>()
    {
        [DeviceTypeNodeName.Plugin] = DeviceType.Plugin,
        [DeviceTypeNodeName.AuPlugin] = DeviceType.Plugin,
        [DeviceTypeNodeName.MaxForLiveAudioEffect] = DeviceType.MaxForLive,
        [DeviceTypeNodeName.MaxForLiveInstrument] = DeviceType.MaxForLive,
        [DeviceTypeNodeName.MaxForLiveMidiEffect] = DeviceType.MaxForLive
    };

    // public DrumRackDeviceExtractor(ILogger<DrumRackDeviceExtractor> logger, IDictionary<DeviceType, IDeviceExtractor> deviceExtractors) : base(logger)
    public DrumRackDeviceExtractor(ILogger<DrumRackDeviceExtractor> logger) : base(logger)
    {
        // this.deviceExtractors = deviceExtractors;
    }

    protected override IDevice CreateDevice()
    {
        return new DrumRackDevice();
    }

    public override IDevice ExtractFromXml(XPathNavigator deviceNode)
    {
        var device = (DrumRackDevice)base.ExtractFromXml(deviceNode);

        //TODO: Implement the branches (chains)!
        // Branches
        // ReturnBranches

        // var devicesFromDrumBranches = GetDevicesFromDrumBranches(deviceNode);
        // var devicesFromReturnBranches = GetDevicesFromReturnBranches(deviceNode);

        // device.AddDevices(devicesFromDrumBranches);
        // device.AddDevices(devicesFromReturnBranches);

        return device;
    }

    // private IList<IDevice> GetDevicesFromDrumBranches(XPathNavigator nav)
    // {

    //     // /Branches/DrumBranch[1]/DeviceChain/MidiToAudioDeviceChain/Devices/OriginalSimpler
    //     logger.LogDebug("----");
    //     logger.LogDebug("Exctracting devices of DrumRack drum chains from XML...");

    //     var devices = new List<IDevice>();
    //     var devicesIterator = nav.Select(@"Branches/DrumBranch/DeviceChain/MidiToAudioDeviceChain/Devices");
    //     devicesIterator.MoveNext();

    //     //TODO: está muito similar à um DeviceExtractionHandler. Verificar como fazer pra reusar!

    //     if (devicesIterator.Current?.HasChildren ?? false)
    //     {
    //         if (devicesIterator.Current.MoveToFirstChild())
    //         {
    //             // Get first device
    //             var deviceNode = devicesIterator.Current;
    //             var device = ExtractDeviceFromNode(deviceNode);
    //             devices.Add(device);

    //             if (device is BaseRackDevice rackDevice)
    //             {
    //                 var childen = rackDevice.ChildrenDevices;
    //                 devices.AddRange(childen.AsEnumerable());
    //             }

    //             // Iterate through all other devices
    //             while (devicesIterator.Current.MoveToNext())
    //             {
    //                 deviceNode = devicesIterator.Current;
    //                 device = ExtractDeviceFromNode(deviceNode);
    //                 devices.Add(device);

    //                 if (device is BaseRackDevice rackDevice2)
    //                 {
    //                     var childen = rackDevice2.ChildrenDevices;
    //                     devices.AddRange(childen.AsEnumerable());
    //                 }
    //             }
    //         }
    //     }

    //     return devices;
    // }

    // private IList<IDevice> GetDevicesFromReturnBranches(XPathNavigator deviceNode)
    // {
    //     // /ReturnBranches/ReturnBranch[1]/DeviceChain/AudioToAudioDeviceChain/Devices/Delay
    //     throw new NotImplementedException();
    // }

    // private IDevice ExtractDeviceFromNode(XPathNavigator deviceNode)
    // {
    //     var type = GetDeviceTypeByDeviceNodeName(deviceNode.Name);
    //     var extractor = GetDeviceExtractorByDeviceType(type);

    //     return extractor.ExtractFromXml(deviceNode);
    // }

    // private DeviceType GetDeviceTypeByDeviceNodeName(string deviceNodeName)
    // {
    //     logger.LogDebug("Getting device type by device node name ({DeviceNodeName})...", deviceNodeName);

    //     var deviceNodeNameUpper = deviceNodeName.ToUpperInvariant();
    //     DeviceType type;

    //     if (deviceTypesByNodeDesc.TryGetValue(deviceNodeNameUpper, out type))
    //         return type;

    //     return DeviceType.Stock;
    // }

    // private IDeviceExtractor GetDeviceExtractorByDeviceType(DeviceType type)
    // {
    //     logger.LogDebug("Getting device extractor by device type ({@DeviceType})...", type);

    //     var extractor = deviceExtractors[type];

    //     logger.LogDebug("Found device extractor: {@DeviceExtractor})", extractor);

    //     return extractor;
    // }
}