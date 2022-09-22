using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.Queries;

public class LiveProjectQuery
{
    public LiveProjectQuery()
    {
        Names = Array.Empty<string>();
        Formats = Array.Empty<PluginFormat>();
        Sorts = Array.Empty<DeviceSort>();
        Creators = Array.Empty<string>();
        MinorVersions = Array.Empty<string>();
        MajorVersions = Array.Empty<string>();
        Tempos = Array.Empty<double>();
    }

    public IReadOnlyCollection<string> Names { get; set; }

    public IReadOnlyCollection<PluginFormat> Formats { get; set; }

    public IReadOnlyCollection<DeviceSort> Sorts { get; set; }

    public IReadOnlyCollection<string> Creators { get; set; }

    public IReadOnlyCollection<string> MinorVersions { get; set; }

    public IReadOnlyCollection<string> MajorVersions { get; set; }

    public IReadOnlyCollection<double> Tempos { get; set; }
}