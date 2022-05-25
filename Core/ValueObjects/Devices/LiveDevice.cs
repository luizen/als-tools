namespace AlsTools.Core.ValueObjects.Devices;

public class LiveDevice : BaseDevice, IDevice
{
    public LiveDevice(DeviceSort sort) : base(sort, DeviceType.Stock)
    {
    }
}
