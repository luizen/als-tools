using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockRacks;

public class DrumRackDeviceExtractor : BaseRackDeviceExtractor, IStockRackDeviceExtractor
{
    public DrumRackDeviceExtractor(ILogger<DrumRackDeviceExtractor> logger, Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>> deviceTypeExtractors, IDictionary<string, DeviceType> deviceTypesByNodeDesc) : base(logger, deviceTypeExtractors, deviceTypesByNodeDesc)
    {
    }

    public override string XPathDevicesSelector => @".//Branches/DrumBranch/DeviceChain/MidiToAudioDeviceChain/Devices/*";

    protected override IDevice CreateDevice()
    {
        return new DrumRackDevice();
    }
}