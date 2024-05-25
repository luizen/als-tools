using AlsTools.Core.Interfaces;

namespace AlsTools.Core.ValueObjects.ResultSets;

public class DevicesUsageCountResult : IEnabledResultSet
{
    public required string DeviceName { get; set; }

    public int UsageCount { get; set; }

    public bool IsEnabled { get; set; }
}
