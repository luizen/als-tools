
using AlsTools.Core.Entities;

namespace AlsTools.Core.ValueObjects.ResultSets;

public class LiveProjectWithChildrenCountsResult
{
    public required LiveProject Project { get; set; }
    public required int TracksCount { get; set; }
    public required int StockDevicesCount { get; set; }
    public required int PluginsCount { get; set; }
    public required int MaxForLiveDevicesCount { get; set; }
    public required int ScenesCount { get; set; }
    public required int LocatorsCount { get; set; }
}