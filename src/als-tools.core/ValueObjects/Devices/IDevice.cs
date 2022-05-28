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

    // /// <summary>
    // /// The device type
    // /// </summary>
    // DeviceType Type { get; }

    /// <summary>
    /// The device family
    /// </summary>
    DeviceFamily Family { get; }
}
