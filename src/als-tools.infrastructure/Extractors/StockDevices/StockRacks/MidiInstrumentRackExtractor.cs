using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;

namespace AlsTools.Infrastructure.Extractors.StockDevices.StockRacks;

public class MidiInstrumentRackExtractor : BaseStockDeviceExtractor, IStockRackExtractor
{
    public MidiInstrumentRackExtractor(ILogger<MidiInstrumentRackExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new InstrumentRackDevice();
    }
}