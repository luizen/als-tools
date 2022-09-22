using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.Queries;

public class LiveProjectQuery
{
    public LiveProjectQuery()
    {
        Names = new List<string>();
        Formats = new List<PluginFormat>();
        Sorts = new List<DeviceSort>();
        Creators = new List<string>();
        MinorVersions = new List<string>();
        MajorVersions = new List<string>();
        Tempos = new List<double>();
    }

    public List<string> Names { get; }

    public List<PluginFormat> Formats { get; }

    public List<DeviceSort> Sorts { get; }

    public List<string> Creators { get; }

    public List<string> MinorVersions { get; }

    public List<string> MajorVersions { get; }

    public List<double> Tempos { get; }
}