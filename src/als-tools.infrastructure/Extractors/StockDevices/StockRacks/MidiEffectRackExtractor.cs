using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;

namespace AlsTools.Infrastructure.Extractors.StockDevices.StockRacks;

public class MidiEffectRackExtractor : BaseStockDeviceExtractor, IStockRackExtractor
{
    public MidiEffectRackExtractor(ILogger<MidiEffectRackExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new MidiEffectRackDevice();
    }
}