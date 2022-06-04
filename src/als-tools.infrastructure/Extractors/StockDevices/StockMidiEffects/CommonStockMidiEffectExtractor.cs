using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.StockDevices.StockMidiEffects;

public class CommonStockMidiEffectExtractor : BaseStockDeviceExtractor, ICommonStockMidiEffectExtractor
{
    public CommonStockMidiEffectExtractor(ILogger<CommonStockMidiEffectExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new StockDevice(DeviceSort.MidiEffect);
    }
}