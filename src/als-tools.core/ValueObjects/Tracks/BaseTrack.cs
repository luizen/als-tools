using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.ValueObjects.Tracks;

public abstract class BaseTrack(TrackType type) : ITrack
{
    public const int DefaultGroupId = -1;

    private const int UndefinedTrackGroupId = -1;

    public int? Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string? EffectiveName { get; set; }

    public TrackType Type { get; set; } = type;

    public IList<StockDevice> StockDevices { get; set; } = [];

    public IList<PluginDevice> Plugins { get; set; } = [];

    public IList<MaxForLiveDevice> MaxForLiveDevices { get; set; } = [];

    public IList<SampleRef> Samples { get; set; } = [];

    public string Annotation { get; set; } = string.Empty;

    // public GroupTrack? ParentGroupTrack { get; set; }

    // public bool IsPartOfGroup => ParentGroupTrack != null;

    public bool IsPartOfGroup => TrackGroupId != UndefinedTrackGroupId;

    public TrackDelay TrackDelay { get; set; } = new();

    public int TrackGroupId { get; set; }

    public bool? IsFrozen { get; set; }

    public bool? IsMuted { get; set; }

    public bool? IsSoloed { get; set; }

    public bool IsGroupTrack => Type == TrackType.Group;

    public bool IsAudioTrack => Type == TrackType.Audio;

    public bool IsMidiTrack => Type == TrackType.Midi;

    public bool IsReturnGroupTrack => Type == TrackType.Return;

    public bool IsMasterTrack => Type == TrackType.Master;

    public LiveColor Color { get; set; }

    public void AddDevice(IDevice device)
    {
        ArgumentNullException.ThrowIfNull(device);

        //TODO: should I get rid of the specific collections (stock, plugins, max4live) and put all devices in a single collection?
        if (device.Family.Type == DeviceType.Plugin)
            Plugins.Add((PluginDevice)device);
        else if (device.Family.Type == DeviceType.Stock)
            StockDevices.Add((StockDevice)device);
        else
            MaxForLiveDevices.Add((MaxForLiveDevice)device);
    }

    public void AddDevices(IEnumerable<IDevice> devices)
    {
        foreach (var device in devices)
            AddDevice(device);
    }

    public void AddSamples(IEnumerable<SampleRef> samples)
    {
        foreach (var sample in samples)
            AddSample(sample);
    }

    public void AddSample(SampleRef sample)
    {
        ArgumentNullException.ThrowIfNull(sample);

        Samples.Add(sample);
    }
}