namespace AlsTools.Core.Filters;

public class DeviceFilter
{
    public DeviceFilter()
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