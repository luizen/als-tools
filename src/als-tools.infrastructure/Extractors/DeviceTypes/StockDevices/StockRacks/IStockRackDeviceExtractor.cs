namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockRacks;

public interface IStockRackDeviceExtractor : IStockDeviceExtractor
{
    string XPathDevicesSelector { get; }
}