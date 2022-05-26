namespace AlsTools.Core.ValueObjects.Devices;

/// <summary>
/// An unknown live device that could not be reckognized. This could be because an older version of device is being used in the Live project.
/// </summary>
public class UnknownLiveDevice : LiveDevice, IDevice
{
    public UnknownLiveDevice(string xmlNodeName) : base(DeviceSort.Unknown)
    {
        XmlNodeName = xmlNodeName;
    }

    /// <summary>
    /// The node value used to identify the device name
    /// </summary>
    public string XmlNodeName { get; private set; }
}
