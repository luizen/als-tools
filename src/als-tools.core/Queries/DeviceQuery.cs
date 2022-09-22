using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.Queries;

public class DeviceQuery
{
    public DeviceQuery()
    {
        Names = new List<string>();
        UserNames = new List<string>();
        Annotations = new List<string>();
        Families = new List<DeviceFamily>();
    }

    public List<string> Names { get; }

    public List<string> UserNames { get; }

    public List<string> Annotations { get; }

    public List<DeviceFamily> Families { get; }
}