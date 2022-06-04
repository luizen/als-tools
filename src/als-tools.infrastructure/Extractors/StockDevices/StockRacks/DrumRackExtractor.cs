using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Core.ValueObjects.Devices.Racks;

namespace AlsTools.Infrastructure.Extractors.StockDevices.StockRacks;

public class DrumRackExtractor : BaseStockDeviceExtractor, IStockRackExtractor
{
    public DrumRackExtractor(ILogger<DrumRackExtractor> logger) : base(logger)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new DrumRackDevice();
    }
}