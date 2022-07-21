using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;
using AlsTools.Infrastructure.XmlNodeNames;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockRacks;

public class MidiInstrumentRackDeviceExtractor : BaseRackDeviceExtractor, IStockRackDeviceExtractor
{
    public MidiInstrumentRackDeviceExtractor(ILogger<MidiInstrumentRackDeviceExtractor> logger, Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>> deviceTypeExtractors, IDictionary<string, DeviceType> deviceTypesByNodeDesc) : base(logger, deviceTypeExtractors, deviceTypesByNodeDesc)
    {
    }

    public override string XPathDevicesSelector => @"./Branches/InstrumentBranch/DeviceChain/MidiToAudioDeviceChain/Devices/*";

    protected override IDevice CreateDevice()
    {
        return new InstrumentRackDevice();
    }
}