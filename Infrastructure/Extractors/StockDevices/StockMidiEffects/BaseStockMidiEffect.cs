using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Infrastructure.Extractors.StockDevices;

namespace AlsTools.Infrastructure.FileSystem.Extractors.StockDevices.StockAudioEffects;

public class BaseStockMidiEffect : BaseStockDeviceExtractor, IBaseStockMidiEffect
{
    private readonly ILogger<BaseStockMidiEffect> logger;

    public BaseStockMidiEffect(ILogger<BaseStockMidiEffect> logger) : base(logger, DeviceSort.MidiEffect)
    {
        this.logger = logger;
    }
}