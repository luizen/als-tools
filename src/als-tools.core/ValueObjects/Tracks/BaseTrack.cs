using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.ValueObjects.Tracks;

public abstract class BaseTrack : ITrack
{
    public const int DefaultGroupId = -1;

    private const int UndefinedTrackGroupId = -1;

    public BaseTrack(TrackType type)
    {
        // stockDevices = new Lazy<List<StockDevice>>();
        // plugins = new Lazy<List<PluginDevice>>();
        // maxForLiveDevices = new Lazy<List<MaxForLiveDevice>>();

        StockDevices = new List<StockDevice>();
        Plugins = new List<PluginDevice>();
        MaxForLiveDevices = new List<MaxForLiveDevice>();

        TrackDelay = new TrackDelay();
        Type = type;
    }

    // private Lazy<List<StockDevice>> stockDevices;

    // private Lazy<List<PluginDevice>> plugins;

    // private Lazy<List<MaxForLiveDevice>> maxForLiveDevices;


    public int? Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string? EffectiveName { get; set; }

    public TrackType Type { get; set; }

    public IList<StockDevice> StockDevices { get; set; }

    public IList<PluginDevice> Plugins { get; set; }

    public IList<MaxForLiveDevice> MaxForLiveDevices { get; set; }

    public string Annotation { get; set; } = string.Empty;

    // public GroupTrack? ParentGroupTrack { get; set; }

    // public bool IsPartOfGroup => ParentGroupTrack != null;

    public bool IsPartOfGroup => TrackGroupId != UndefinedTrackGroupId;

    public TrackDelay TrackDelay { get; set; }

    public int TrackGroupId { get; set; }

    public bool? IsFrozen { get; set; }

    public bool? IsMuted { get; set; }

    public bool? IsSoloed { get; set; }

    public bool IsGroupTrack => Type == TrackType.Group;

    public bool IsAudioTrack => Type == TrackType.Audio;

    public bool IsMidiTrack => Type == TrackType.Midi;

    public bool IsReturnGroupTrack => Type == TrackType.Return;

    public bool IsMasterTrack => Type == TrackType.Master;

    public int Color { get; set; }

    public void AddDevice(IDevice device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device));

        //TODO: should I get rid of the specific collections (stock, plugins, max4live) and put all devices in a single collection?
        if (device.Family.Type == DeviceType.Plugin)
            // plugins.Value.Add((PluginDevice)device);
            Plugins.Add((PluginDevice)device);
        else if (device.Family.Type == DeviceType.Stock)
            // stockDevices.Value.Add((StockDevice)device);
            StockDevices.Add((StockDevice)device);
        else
            // maxForLiveDevices.Value.Add((MaxForLiveDevice)device);
            MaxForLiveDevices.Add((MaxForLiveDevice)device);
    }

    public void AddDevices(IEnumerable<IDevice> devices)
    {
        foreach (var device in devices)
            AddDevice(device);
    }
}