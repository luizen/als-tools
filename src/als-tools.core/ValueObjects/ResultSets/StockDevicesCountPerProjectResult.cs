namespace AlsTools.Core.ValueObjects.ResultSets;

public class StockDevicesCountPerProjectResult
{
    public string ProjectName { get; set; } = string.Empty;

    public string ProjectPath { get; set; } = string.Empty;

    public int StockDevicesCount { get; set; }
}
