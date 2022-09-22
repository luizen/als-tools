using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.Queries;

public class PluginQuery
{
    public PluginQuery()
    {
        Names = new List<string>();
        Formats = new List<PluginFormat>();
        Sorts = new List<DeviceSort>();
    }

    public List<string> Names { get; }

    public List<PluginFormat> Formats { get; }

    public List<DeviceSort> Sorts {get; }
}