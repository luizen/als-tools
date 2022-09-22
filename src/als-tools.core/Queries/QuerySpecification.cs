namespace AlsTools.Core.Queries;

public class QuerySpecification
{
    private static readonly QuerySpecification emptyInstance = new QuerySpecification();

    public static QuerySpecification Empty => emptyInstance;

    public LiveProjectQuery? LiveProjectQuery { get; set; }

    public DeviceQuery? DeviceQuery { get; set; }

    public TrackQuery? TrackQuery { get; set; }

    public PluginQuery? PluginQuery { get; set; }
}