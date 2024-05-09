namespace AlsTools.Core.ValueObjects.Devices;

public record PluginDevice : BaseDevice
{
    public PluginDevice(DeviceSort sort, PluginFormat format, string name) : base(sort, DeviceType.Plugin)
    {
        Format = format;
        Name = name;
    }

    public PluginFormat Format { get; }
}
