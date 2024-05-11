using System.ComponentModel.DataAnnotations;
using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.ValueObjects.Tracks;

public abstract class BaseTrack : ITrack
{
    public BaseTrack(TrackType type)
    {
        Plugins = new List<PluginDevice>();
        Type = type;
    }

    [Key]
    public int Id { get; set; }
    // public int LiveProjectId { get; set; }
    // public LiveProject LiveProject { get; set; }

    public string UserName { get; set; } = string.Empty;

    public TrackType Type { get; set; }

    public IList<PluginDevice> Plugins { get; set; }
}