using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.StockDevices.StockAudioEffects;

public class CommonStockAudioEffectExtractor : BaseStockDeviceExtractor, ICommonStockAudioEffectExtractor
{
    public CommonStockAudioEffectExtractor(ILogger<CommonStockAudioEffectExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new StockDevice(DeviceSort.AudioEffect);
    }
}