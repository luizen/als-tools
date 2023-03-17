namespace AlsTools.Core.Queries;

public class QuerySpecification
{
    public QuerySpecification()
    {
        LiveProjectQuery = new();
        PluginQuery = new();
        TrackQuery = new();
        SceneQuery = new();
        StockDeviceQuery = new();
        MaxForLiveDeviceQuery = new();
    }

    private static readonly QuerySpecification emptyInstance = new QuerySpecification();

    public static QuerySpecification Empty => emptyInstance;


    public LiveProjectQuery LiveProjectQuery { get; set; }
    
    public SceneQuery SceneQuery { get; set; }

    public DeviceQuery StockDeviceQuery { get; set; }

    public DeviceQuery MaxForLiveDeviceQuery { get; set; }

    public TrackQuery TrackQuery { get; set; }

    public PluginQuery PluginQuery { get; set; }
}