using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.Queries;

public class DeviceQuery
{
    public DeviceQuery()
    {
        Names = Array.Empty<string>();
        UserNames = Array.Empty<string>();
        Annotations = Array.Empty<string>();
        Families = Array.Empty<DeviceFamily>();
    }

    public IReadOnlyCollection<string> Names { get; set; }

    public IReadOnlyCollection<string> UserNames { get; set; }

    public IReadOnlyCollection<string> Annotations { get; set; }

    public IReadOnlyCollection<DeviceFamily> Families { get; set; }
}