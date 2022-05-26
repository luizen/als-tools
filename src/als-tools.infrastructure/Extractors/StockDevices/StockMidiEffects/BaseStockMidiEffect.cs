using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.StockDevices.StockMidiEffects;

public class BaseStockMidiEffect : BaseStockDeviceExtractor, IBaseStockMidiEffect
{
    private readonly ILogger<BaseStockMidiEffect> logger;

    public BaseStockMidiEffect(ILogger<BaseStockMidiEffect> logger) : base(logger, DeviceSort.MidiEffect)
    {
        this.logger = logger;
    }
}