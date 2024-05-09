using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.ValueObjects.Tracks;

public interface ITrack
{
    int Id { get; set; }

    string UserName { get; set; }

    TrackType Type { get; set; }

    IList<PluginDevice> Plugins { get; set; }
}