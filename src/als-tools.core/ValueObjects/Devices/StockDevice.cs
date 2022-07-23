namespace AlsTools.Core.ValueObjects.Devices;

public class StockDevice : BaseDevice
{
    public StockDevice(DeviceSort sort) : base(sort, DeviceType.Stock)
    {
    }
}
