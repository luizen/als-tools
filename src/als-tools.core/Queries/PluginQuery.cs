using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.Queries;

public class PluginQuery
{
    public PluginQuery()
    {
        Names = new List<string>();
        Formats = Array.Empty<PluginFormat>();
        Sorts = Array.Empty<DeviceSort>();

    }

    public IList<string> Names { get; set; }

    public IReadOnlyCollection<PluginFormat> Formats { get; set; }

    public IReadOnlyCollection<DeviceSort> Sorts {get; set; }
}