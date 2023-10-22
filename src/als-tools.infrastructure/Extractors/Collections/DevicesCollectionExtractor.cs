using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;
using AlsTools.Infrastructure.Extractors.DeviceTypes;
using AlsTools.Infrastructure.XmlNodeNames;

namespace AlsTools.Infrastructure.Extractors.Collections;

/// <summary>
/// Interface defining a collection extractor specific for IDevices
/// </summary>
public interface IDevicesCollectionExtractor : ICollectionExtractor<IDevice>
{
}

/// <summary>
/// Collection extractor specific for IDevices
/// </summary>
public class DevicesCollectionExtractor : IDevicesCollectionExtractor
{
    private readonly ILogger<DevicesCollectionExtractor> logger;
    private readonly Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>> deviceTypeExtractors;
    private readonly XpathExtractorHelper xpathExtractorHelper;

    private static readonly IDictionary<string, DeviceType> deviceTypesByNodeDesc = new Dictionary<string, DeviceType>()
    {
        [DeviceTypeNodeName.Plugin] = DeviceType.Plugin,
        [DeviceTypeNodeName.AuPlugin] = DeviceType.Plugin,
        [DeviceTypeNodeName.MaxForLiveAudioEffect] = DeviceType.MaxForLive,
        [DeviceTypeNodeName.MaxForLiveInstrument] = DeviceType.MaxForLive,
        [DeviceTypeNodeName.MaxForLiveMidiEffect] = DeviceType.MaxForLive
    };

    public DevicesCollectionExtractor(ILogger<DevicesCollectionExtractor> logger, Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>> deviceTypeExtractors, XpathExtractorHelper xpathExtractorHelper)
    {
        if (deviceTypeExtractors == null || deviceTypeExtractors.Value.Count == 0)
            throw new ArgumentNullException(nameof(deviceTypeExtractors));

        this.logger = logger;
        this.deviceTypeExtractors = deviceTypeExtractors;
        this.xpathExtractorHelper = xpathExtractorHelper;
    }

    public IReadOnlyList<IDevice> ExtractFromXml(XPathNavigator nav)
    {
        logger.LogDebug("----");
        logger.LogDebug("Exctracting devices from XML...");

        string[] devicesNodeExpressions =
            {
                @"DeviceChain/DeviceChain/Devices",
                @"MasterChain/DeviceChain/Devices" // for older versions
            };

        // Try to get the <Devices> node
        var node = xpathExtractorHelper.TryGetOneOfXpathNodes(nav, devicesNodeExpressions);
        if (node == null)
            return Enumerable.Empty<IDevice>().ToArray();

        var devices = new List<IDevice>();

        // Currently 'node' should be the <Devices> node
        if (node.HasChildren)
        {
            // This also works: node.Select(@"./*");

            if (node.MoveToFirstChild())
            {
                // Now 'node' should be the first device under the <Devices> node

                // Get first device
                ExtractDeviceIntoDevicesList(devices, node);

                // Iterate through all other sibling devices
                while (node.MoveToNext())
                {
                    ExtractDeviceIntoDevicesList(devices, node);
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

        if (deviceTypesByNodeDesc.TryGetValue(deviceNodeNameUpper, out DeviceType type))
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