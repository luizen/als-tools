namespace AlsTools.Core.ValueObjects.Devices;

public class PluginDevice : BaseDevice
{
    public PluginDevice(DeviceSort sort, PluginFormat format) : base(sort, DeviceType.Plugin)
    {
        Format = format;
    }

    public PluginFormat Format { get; protected set; }
}
