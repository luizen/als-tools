using AlsTools.Core.Extensions;
using AlsTools.Core.Models.Tracks;

namespace AlsTools.Core.Models.Devices;

public abstract class BaseDevice
{
    public static IList<BaseDevice> EmptyDevicesList = Enumerable.Empty<BaseDevice>().ToList();

    public BaseDevice(DeviceSort deviceSort, DeviceType type)
    {
        Family = new DeviceFamily(type, deviceSort);
    }

    public BaseDevice(DeviceFamily family)
    {
        Family = family;
    }

    /// <summary>
    /// The foreign key to the track this device belongs to.
    /// </summary>
    public int FkTrackId { get; set; }

    /// <summary>
    /// The track this device belongs to.
    /// </summary>
    public BaseTrack FkTrack { get; set; } = null!;

    /// <summary>
    /// Internal (persistency related) ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The device intenal Id attribute
    /// </summary>
    public int DeviceId { get; set; }

    /// <summary>
    /// The device original, factory name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The name the user has given to the device
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The device info text/annotation/notes
    /// </summary>
    public string Annotation { get; set; } = string.Empty;

    /// <summary>
    /// The device family
    /// </summary>
    public DeviceFamily Family { get; protected set; }

    /// <summary>
    /// Whether the device is turned on or off
    /// </summary>
    public bool? IsOn { get; set; } = true;

    /// <summary>
    /// Helper property that calculates whether the device is contained within a rack or not
    /// </summary>
    public bool HasParentRack => ParentRackDevice.HasValue;

    /// <summary>
    /// Contains (optional) information about parent rack device
    /// </summary>
    public ParentDeviceDescriptor? ParentRackDevice { get; private set; } = null;

    /// <summary>
    /// Helper property that calculates whether this device is enabled or not. It takes into consideration both properties <see cref="IsOn"/> and <see cref="ParentDeviceDescriptor.IsOn"/>. 
    /// </summary>
    public bool IsEnabled => IsOn.HasValueTrue() && (!ParentRackDevice.HasValue || IsOn.HasValueTrue());

    /// <summary>
    /// Whether this is a device that groups other devices (eg.: Rack device)
    /// </summary>
    public virtual bool IsGroupDevice => false;

    /// <summary>
    /// Initializes the <see cref="ParentRackDevice"/> property, marking this device as having a parent
    /// </summary>
    /// <param name="isParentRackOn">Whether the parent rack device is on/off</param>
    public void DefineParentRack(bool? isParentRackOn)
    {
        ParentRackDevice = new ParentDeviceDescriptor(isParentRackOn);
    }
}