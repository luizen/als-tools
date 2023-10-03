namespace AlsTools.Core.ValueObjects.Devices;

public interface IDevice
{
    /// <summary>
    /// The device intenal Id attribute
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// The device original, factory name
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// The name the user has given to the device
    /// </summary>
    string UserName { get; set; }

    /// <summary>
    /// The device info text/annotation/notes
    /// </summary>
    string Annotation { get; set; }

    /// <summary>
    /// The device family
    /// </summary>
    DeviceFamily Family { get; }

    /// <summary>
    /// Helper property that calculates whether the device is contained within a rack or not
    /// </summary>
    bool HasParentRack { get; }

    /// <summary>
    /// Contains (optional) information about parent rack device
    /// </summary>
    ParentDeviceDescriptor? ParentRackDevice { get; }

    /// <summary>
    /// Whether the device is turned on or off
    /// </summary>
    bool IsOn { get; set; }

    /// <summary>
    /// Helper property that calculates whether this device is enabled or not. It takes into consideration both properties <see cref="IsOn"/> and <see cref="ParentDeviceDescriptor.IsOn"/>. 
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Whether this is a device that groups other devices (eg.: Rack device)
    /// </summary>
    bool IsGroupDevice { get; }

    /// <summary>
    /// Initializes the <see cref="ParentRackDevice"/> property, marking this device as having a parent
    /// </summary>
    /// <param name="isParentRackOn">Whether the parent rack device is on/off</param>
    void DefineParentRack(bool isParentRackOn);
}
