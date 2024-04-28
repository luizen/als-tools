namespace AlsTools.Core.ValueObjects.ResultSets;

public class DevicesUsageCountResult
{
    public required string DeviceName { get; set; }

    public int UsageCount { get; set; }
}
