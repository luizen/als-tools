namespace AlsTools.Core.ValueObjects.Devices;

public record MaxForLiveDevice : BaseDevice
{
    public MaxForLiveDevice(DeviceSort sort) : base(sort, DeviceType.MaxForLive)
    {
    }
}
