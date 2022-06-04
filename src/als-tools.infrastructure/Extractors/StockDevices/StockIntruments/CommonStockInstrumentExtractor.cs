using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.StockDevices.StockInstruments;

public class CommonStockInstrumentExtractor : BaseStockDeviceExtractor, ICommonStockInstrumentExtractor
{
    public CommonStockInstrumentExtractor(ILogger<CommonStockInstrumentExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new StockDevice(DeviceSort.MidiInstrument);
    }
}