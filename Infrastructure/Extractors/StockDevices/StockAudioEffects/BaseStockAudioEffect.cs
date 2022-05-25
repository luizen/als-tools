using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Infrastructure.Extractors.StockDevices;

namespace AlsTools.Infrastructure.FileSystem.Extractors.StockDevices.StockAudioEffects;

public class BaseStockAudioEffect : BaseStockDeviceExtractor, IBaseStockAudioEffect
{
    private readonly ILogger<BaseStockAudioEffect> logger;

    public BaseStockAudioEffect(ILogger<BaseStockAudioEffect> logger) : base(logger, DeviceSort.AudioEffect)
    {
        this.logger = logger;
    }
}