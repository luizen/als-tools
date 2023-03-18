namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockAudioEffects;

public interface ICommonStockAudioEffectDeviceExtractor : IStockDeviceExtractor
{
}

public class CommonStockAudioEffectDeviceExtractor : BaseStockDeviceExtractor, ICommonStockAudioEffectDeviceExtractor
{
    public CommonStockAudioEffectDeviceExtractor(ILogger<CommonStockAudioEffectDeviceExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new StockDevice(DeviceSort.AudioEffect);
    }
}