namespace AlsTools.Core.ValueObjects.ResultSets;

public class PluginsCountPerProjectResult
{
    public string ProjectName { get; set; } = string.Empty;

    public string ProjectPath { get; set; } = string.Empty;

    public int PluginsCount { get; set; }
}
