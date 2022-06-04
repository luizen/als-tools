using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;

namespace AlsTools.Infrastructure.Extractors.StockDevices.StockRacks;

public class AudioEffectRackExtractor : BaseStockDeviceExtractor, IStockRackExtractor
{
    public AudioEffectRackExtractor(ILogger<AudioEffectRackExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new AudioEffectRackDevice();
    }
}