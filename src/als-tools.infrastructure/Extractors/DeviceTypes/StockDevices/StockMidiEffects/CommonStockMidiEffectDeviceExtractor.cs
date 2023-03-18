namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockMidiEffects;

public interface ICommonStockMidiEffectDeviceExtractor : IStockDeviceExtractor
{
}

public class CommonStockMidiEffectDeviceExtractor : BaseStockDeviceExtractor, ICommonStockMidiEffectDeviceExtractor
{
    public CommonStockMidiEffectDeviceExtractor(ILogger<CommonStockMidiEffectDeviceExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new StockDevice(DeviceSort.MidiEffect);
    }
}