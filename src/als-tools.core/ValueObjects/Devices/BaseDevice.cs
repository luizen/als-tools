namespace AlsTools.Core.ValueObjects.Devices;

public abstract class BaseDevice : IDevice
{
    public static IList<IDevice> EmptyDevicesList = Enumerable.Empty<IDevice>().ToList();

    public BaseDevice(DeviceSort deviceSort, DeviceType type)
    {
        Family = new DeviceFamily(type, deviceSort);
    }

    public BaseDevice(DeviceFamily family)
    {
        Family = family;
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Annotation { get; set; } = string.Empty;

    public DeviceFamily Family { get; protected set; }
}
