using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockInstruments;

public interface ICommonStockInstrumentDeviceExtractor : IStockDeviceExtractor
{
}

public class CommonStockInstrumentDeviceExtractor : BaseStockDeviceExtractor, ICommonStockInstrumentDeviceExtractor
{
    public CommonStockInstrumentDeviceExtractor(ILogger<CommonStockInstrumentDeviceExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new StockDevice(DeviceSort.MidiInstrument);
    }
}