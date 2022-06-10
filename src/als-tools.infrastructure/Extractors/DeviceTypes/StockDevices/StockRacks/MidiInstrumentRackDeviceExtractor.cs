using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockRacks;

public class MidiInstrumentRackDeviceExtractor : BaseStockDeviceExtractor, IStockRackDeviceExtractor
{
    public MidiInstrumentRackDeviceExtractor(ILogger<MidiInstrumentRackDeviceExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new InstrumentRackDevice();
    }
}