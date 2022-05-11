namespace AlsTools.Core.ValueObjects.Devices
{
    public class LiveDevice : BaseDevice, IDevice
    {
        public LiveDevice() : base(DeviceType.LiveDevice)
        {
        }
    }
}
