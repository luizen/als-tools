namespace AlsTools.Core.Entities.Devices
{
    public interface IDevice
    {
        string Name { get; set; }

        DeviceType Type { get; }
    }
}
