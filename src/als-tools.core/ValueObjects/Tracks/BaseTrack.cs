using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.ValueObjects.Tracks;

public abstract class BaseTrack : ITrack
{
    public const int DefaultGroupId = -1;

    public BaseTrack(TrackType type)
    {
        stockDevices = new Lazy<List<StockDevice>>();
        plugins = new Lazy<List<PluginDevice>>();
        maxForLiveDevices = new Lazy<List<MaxForLiveDevice>>();

        TrackDelay = new TrackDelay();
        Type = type;
    }

    private Lazy<List<StockDevice>> stockDevices;

    private Lazy<List<PluginDevice>> plugins;

    private Lazy<List<MaxForLiveDevice>> maxForLiveDevices;

    public int? Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string EffectiveName { get; set; } = string.Empty;

    public TrackType Type { get; set; }

    public IReadOnlyList<StockDevice> StockDevices
    {
        get
        {
            return stockDevices.Value.AsReadOnly();
        }
    }

    public IReadOnlyList<PluginDevice> Plugins
    {
        get
        {
            return plugins.Value.AsReadOnly();
        }
    }

    public IReadOnlyList<MaxForLiveDevice> MaxForLiveDevices
    {
        get
        {
            return maxForLiveDevices.Value.AsReadOnly();
        }
    }

    public string Annotation { get; set; } = string.Empty;

    public GroupTrack? ParentGroupTrack { get; set; }

    public bool IsPartOfGroup => ParentGroupTrack != null;

    public TrackDelay TrackDelay { get; set; }

    public int TrackGroupId { get; set; }

    public bool? IsFrozen { get; set; }

    public void AddDevice(IDevice device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device));

        //TODO: should I get rid of the specific collections (stock, plugins, max4live) and put all devices in a single collection?
        if (device.Family.Type == DeviceType.Plugin)
            plugins.Value.Add((PluginDevice)device);
        else if (device.Family.Type == DeviceType.Stock)
            stockDevices.Value.Add((StockDevice)device);
        else
            maxForLiveDevices.Value.Add((MaxForLiveDevice)device);
    }

    public void AddDevices(IEnumerable<IDevice> devices)
    {
        foreach (var device in devices)
            AddDevice(device);
    }
}