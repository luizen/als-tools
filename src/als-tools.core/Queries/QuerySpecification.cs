namespace AlsTools.Core.Queries;

public class QuerySpecification
{
    public LiveProjectQuery? LiveProjectQuery { get; set; }

    public DeviceQuery? DeviceQuery { get; set; }

    public TrackQuery? TrackQuery { get; set; }

    public PluginQuery? PluginQuery { get; set; }
}