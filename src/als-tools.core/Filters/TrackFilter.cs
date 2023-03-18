namespace AlsTools.Core.Filters;

public class TrackFilter
{
    public TrackFilter()
    {
        UserNames = new List<string>();
        EffectiveNames = new List<string>();
        Annotations = new List<string>();
        Types = new List<TrackType>();
        Delays = new List<TrackDelay>();
    }

    public List<string> UserNames { get; }

    public List<string> EffectiveNames { get; }

    public List<TrackType> Types { get; }

    public List<string> Annotations { get; }

    public bool? IsFrozen { get; set; }

    public bool? IsPartOfGroup { get; set; }

    public List<TrackDelay> Delays { get; }
    
    public bool? ContainsAnyMaxForLiveDevices { get; set; }
    public bool? ContainsAnyPlugins { get; set; }
    public bool? ContainsAnyStockDevices { get; set; }
    public bool? ContainsNumberOfMaxForLiveDevices { get; set; }
    public int? ContainsNumberOfPlugins { get; set; }
}