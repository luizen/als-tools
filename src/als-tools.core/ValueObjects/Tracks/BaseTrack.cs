using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.ValueObjects.Tracks;

public abstract class BaseTrack : ITrack
{
    public BaseTrack(TrackType type)
    {
        Plugins = new List<PluginDevice>();
        Type = type;
    }

    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public TrackType Type { get; set; }

    public IList<PluginDevice> Plugins { get; set; }
}