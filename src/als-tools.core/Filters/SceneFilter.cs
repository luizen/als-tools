namespace AlsTools.Core.Filters;

public class SceneFilter
{
    public SceneFilter()
    {
        Names = new List<string>();
        Tempos = new List<int>();
        Annotations = new List<string>();
    }

    public List<string> Names { get; }

    public List<int> Tempos { get; }

    public List<string> Annotations { get; set; }
}