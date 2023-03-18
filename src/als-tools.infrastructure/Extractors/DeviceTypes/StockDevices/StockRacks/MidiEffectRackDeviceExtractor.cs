namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockRacks;

public class MidiEffectRackDeviceExtractor : BaseRackDeviceExtractor, IStockRackDeviceExtractor
{
    public override string XPathDevicesSelector => @"./Branches/MidiEffectBranch/DeviceChain/MidiToMidiDeviceChain/Devices/*";

    public MidiEffectRackDeviceExtractor(ILogger<MidiEffectRackDeviceExtractor> logger, Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>> deviceTypeExtractors, IDictionary<string, DeviceType> deviceTypesByNodeDesc) : base(logger, deviceTypeExtractors, deviceTypesByNodeDesc)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new MidiEffectRackDevice();
    }
}