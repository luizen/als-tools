namespace AlsTools.Core.ValueObjects.ResultSets;

public class ItemsCountPerProjectResult
{
    public string ProjectName { get; set; } = string.Empty;

    public string ProjectPath { get; set; } = string.Empty;

    public int ItemsCount { get; set; }
}
