namespace AlsTools.Core.ValueObjects.Devices
{
    public abstract class BaseDevice : IDevice
    {
        public BaseDevice(DeviceType type)
        {
            Type = type;
        }

        public string Name { get; set; }

        public DeviceType Type { get; protected set; }
    }
}
