namespace AlsTools.Core.ValueObjects.Devices;

public class PluginDevice : BaseDevice, IDevice
{
    public PluginDevice(DeviceSort sort, PluginFormat format) : base(sort, DeviceType.Plugin)
    {
        Format = format;
    }

    public PluginFormat Format { get; protected set; }
}
