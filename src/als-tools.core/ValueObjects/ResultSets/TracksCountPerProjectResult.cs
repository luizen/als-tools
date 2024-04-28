namespace AlsTools.Core.ValueObjects.ResultSets;

public class TracksCountPerProjectResult
{
    public string ProjectName { get; set; } = string.Empty;

    public string ProjectPath { get; set; } = string.Empty;

    public int TracksCount { get; set; }
}
