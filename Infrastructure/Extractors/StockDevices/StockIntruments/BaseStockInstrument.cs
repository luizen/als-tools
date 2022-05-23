using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Infrastructure.Extractors.StockDevices;
using Microsoft.Extensions.Logging;

namespace AlsTools.Infrastructure.FileSystem.Extractors.StockDevices.StockAudioEffects;

public class BaseStockInstrument : BaseStockDeviceExtractor, IBaseStockInstrument
{
    private readonly ILogger<BaseStockInstrument> logger;

    public BaseStockInstrument(ILogger<BaseStockInstrument> logger) : base(logger, DeviceSort.MidiInstrument)
    {
        this.logger = logger;
    }
}