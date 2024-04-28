namespace AlsTools.Core.ValueObjects.ResultSets;

public class PluginsUsageCountResult
{
    public required string PluginName { get; set; }

    public int UsageCount { get; set; }
}
