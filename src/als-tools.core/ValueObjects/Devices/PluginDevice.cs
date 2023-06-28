using System.Diagnostics.CodeAnalysis;

namespace AlsTools.Core.ValueObjects.Devices;

public class PluginDevice : BaseDevice, ICloneable
{
    public PluginDevice(DeviceSort sort, PluginFormat format) : base(sort, DeviceType.Plugin)
    {
        Format = format;
    }

    public PluginFormat Format { get; }

    public string? Path { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}

public class PluginDeviceEqualityComparer : IEqualityComparer<PluginDevice>
{
    public bool Equals(PluginDevice? x, PluginDevice? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (x is null || y is null)
            return false;

        return x.Format == y.Format &&
                x.Name == y.Name;
    }

    public int GetHashCode([DisallowNull] PluginDevice obj)
    {
        int hash = 17;
        hash = hash * 31 + obj.Format.GetHashCode();

        if (obj.Name != null)
        {
            hash = hash * 31 + StringComparer.Ordinal.GetHashCode(obj.Name);
        }

        return hash;
    }

}


public class PluginDevicePathEqualityComparer : IEqualityComparer<PluginDevice>
{
    public bool Equals(PluginDevice? x, PluginDevice? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (x is null || y is null)
            return false;

        return x.Path == y.Path;
    }

    public int GetHashCode([DisallowNull] PluginDevice obj)
    {
        return obj.Path.GetHashCode();
    }

}

