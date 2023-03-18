namespace AlsTools.Core.Filters;

public class LiveProjectFilter
{
    public LiveProjectFilter()
    {
        Names = new List<string>();
        Creators = new List<string>();
        MinorVersions = new List<string>();
        MajorVersions = new List<string>();
        Tempos = new List<double>();
        Paths = new List<string>();
    }

    public List<string> Names { get; }

    public List<string> Creators { get; }

    public List<string> MinorVersions { get; }

    public List<string> MajorVersions { get; }

    public List<double> Tempos { get; }

    public List<string> Paths { get; set; }
}