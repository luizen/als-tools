namespace AlsTools.Core.ValueObjects.Devices;

public abstract record BaseDevice : IDevice
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

    public bool? IsOn { get; set; } = true;

    public bool HasParentRack => ParentRackDevice.HasValue;

    public ParentDeviceDescriptor? ParentRackDevice { get; private set; } = null;

    public bool IsEnabled => IsOn.HasValueTrue() && (!ParentRackDevice.HasValue || IsOn.HasValueTrue());

    public virtual bool IsGroupDevice => false;

    public void DefineParentRack(bool? isParentRackOn)
    {
        ParentRackDevice = new ParentDeviceDescriptor(isParentRackOn);
    }
}