using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.StockDevices.StockAudioEffects;

public class BaseStockAudioEffect : BaseStockDeviceExtractor, IBaseStockAudioEffect
{
    private readonly ILogger<BaseStockAudioEffect> logger;

    public BaseStockAudioEffect(ILogger<BaseStockAudioEffect> logger) : base(logger, DeviceSort.AudioEffect)
    {
        this.logger = logger;
    }
}