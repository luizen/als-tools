namespace AlsTools.Core.ValueObjects.ResultSets;

public class StockDevicesUsageCountResult
{
    public required string StockDeviceName { get; set; }

    public int UsageCount { get; set; }
}
