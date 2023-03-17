namespace AlsTools.Core.Queries;

public class SceneQuery
{
    public SceneQuery()
    {
        Names = new List<string>();
        Tempos = new List<int>();
        Annotations = new List<string>();
    }

    public List<string> Names { get; }

    public List<int> Tempos { get; }

    public List<string> Annotations { get; set; }
}