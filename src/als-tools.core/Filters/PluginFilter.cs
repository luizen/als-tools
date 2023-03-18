namespace AlsTools.Core.Filters;

public class PluginFilter
{
    public PluginFilter()
    {
        Names = new List<string>();
        Formats = new List<PluginFormat>();
        Sorts = new List<DeviceSort>();
    }

    public List<string> Names { get; }

    public List<PluginFormat> Formats { get; }

    public List<DeviceSort> Sorts {get; }
}