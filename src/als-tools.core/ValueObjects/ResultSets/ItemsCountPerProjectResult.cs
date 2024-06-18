using AlsTools.Core.Interfaces;

namespace AlsTools.Core.ValueObjects.ResultSets;

public class ItemsCountPerProjectResult : IEnabledResultSet
{
    public string ProjectId { get; set; } = string.Empty;

    public string ProjectName { get; set; } = string.Empty;

    public string ProjectPath { get; set; } = string.Empty;

    public int ItemsCount { get; set; }

    public bool IsEnabled { get; set; }
}
