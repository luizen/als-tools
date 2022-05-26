namespace AlsTools.Core.ValueObjects.Devices;

public abstract class BaseDevice : IDevice
{
    public BaseDevice(DeviceSort deviceSort, DeviceType type)
    {
        Family = new DeviceFamily(type, deviceSort);
    }

    public BaseDevice(DeviceFamily family)
    {
        Family = family;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string UserName { get; set; }

    public string Annotation { get; set; }

    public DeviceFamily Family { get; protected set; }
}
