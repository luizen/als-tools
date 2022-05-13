namespace AlsTools.Core.ValueObjects.Devices
{
    public interface IDevice
    {
        string Name { get; set; }

        DeviceType Type { get; }
    }
}
