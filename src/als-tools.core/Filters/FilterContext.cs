namespace AlsTools.Core.Filters;

public class FilterContext
{
    public FilterContext()
    {
        LiveProjectFilter = new();
        PluginFilter = new();
        TrackFilter = new();
        SceneFilter = new();
        StockDeviceFilter = new();
        MaxForLiveDeviceFilter = new();
        FilterSettings = new();
    }

    private static readonly FilterContext emptyInstance = new FilterContext();

    public static FilterContext Empty => emptyInstance;


    public FilterSettings FilterSettings  { get; set; }

    public LiveProjectFilter LiveProjectFilter { get; set; }
    
    public SceneFilter SceneFilter { get; set; }

    public DeviceFilter StockDeviceFilter { get; set; }

    public DeviceFilter MaxForLiveDeviceFilter { get; set; }

    public TrackFilter TrackFilter { get; set; }

    public PluginFilter PluginFilter { get; set; }
}