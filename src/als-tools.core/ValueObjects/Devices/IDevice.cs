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
    /// Whether the device is contained within a rack
    /// </summary>
    bool HasParentRack { get; set; }

    /// <summary>
    /// Whether the device is turned on or off
    /// </summary>
    bool IsOn { get; set; }

    /// <summary>
    /// Whether the parent rack where this device is located is turned on or off. This will make the device disabled as well.
    /// </summary>
    bool IsParentRackOn { get; set; }

    /// <summary>
    /// Calculates whether this device is enabled or not. It takes into consideration both properties <see cref="IsOn"/> and <see cref="IsParentRackOn"/>. 
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Whether this is a device that groups other devices (eg.: Rack device)
    /// </summary>
    bool IsGroupDevice { get; }
}
