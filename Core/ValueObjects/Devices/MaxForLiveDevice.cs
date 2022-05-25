namespace AlsTools.Core.ValueObjects.Devices;

public class MaxForLiveDevice : BaseDevice, IDevice
{
    public MaxForLiveDevice(DeviceSort sort) : base(sort, DeviceType.MaxForLive)
    {
    }
}
