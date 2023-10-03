namespace AlsTools.Core.ValueObjects.Devices;

public record StockDevice : BaseDevice
{
    public StockDevice(DeviceSort sort) : base(sort, DeviceType.Stock)
    {
    }
}
