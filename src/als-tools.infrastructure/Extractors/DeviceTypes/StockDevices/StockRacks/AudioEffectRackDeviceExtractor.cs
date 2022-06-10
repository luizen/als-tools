using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockRacks;

public class AudioEffectRackDeviceExtractor : BaseStockDeviceExtractor, IStockRackDeviceExtractor
{
    public AudioEffectRackDeviceExtractor(ILogger<AudioEffectRackDeviceExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new AudioEffectRackDevice();
    }
}