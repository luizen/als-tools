namespace AlsTools.Core.Models.Devices;

public class StockDevice : BaseDevice
{
    public StockDevice(DeviceSort sort) : base(sort, DeviceType.Stock)
    {
    }
}
