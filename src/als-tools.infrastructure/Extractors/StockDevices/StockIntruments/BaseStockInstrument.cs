using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.StockDevices.StockInstruments;

public class BaseStockInstrument : BaseStockDeviceExtractor, IBaseStockInstrument
{
    private readonly ILogger<BaseStockInstrument> logger;

    public BaseStockInstrument(ILogger<BaseStockInstrument> logger) : base(logger, DeviceSort.MidiInstrument)
    {
        this.logger = logger;
    }
}