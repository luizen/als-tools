using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockRacks;

public class MidiEffectRackDeviceExtractor : BaseStockDeviceExtractor, IStockRackDeviceExtractor
{
    public MidiEffectRackDeviceExtractor(ILogger<MidiEffectRackDeviceExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new MidiEffectRackDevice();
    }
}