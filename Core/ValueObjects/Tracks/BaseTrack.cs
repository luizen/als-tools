using System.Collections.Generic;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.ValueObjects.Tracks;

public abstract class BaseTrack : ITrack
{
    public BaseTrack(TrackType type)
    {
        StockDevices = new List<LiveDevice>();
        Plugins = new List<PluginDevice>();
        MaxForLiveDevices = new List<MaxForLiveDevice>();

        TrackDelay = new TrackDelay();
        Type = type;
    }

    // /// <summary>
    // /// The track internal Id (from Id attribute).
    // /// TODO: is it really needed?
    // /// </summary>
    // public int Id { get; set; }

    public string UserName { get; set; }

    public string EffectiveName { get; set; }

    public TrackType Type { get; set; }

    public IList<LiveDevice> StockDevices { get; protected set; }

    public IList<PluginDevice> Plugins { get; protected set; }

    public IList<MaxForLiveDevice> MaxForLiveDevices { get; protected set; }

    public string Annotation { get; set; }

    public GroupTrack ParentGroupTrack { get; set; }

    public bool IsPartOfGroup => ParentGroupTrack != null;

    public TrackDelay TrackDelay { get; set; }
   
    public int? TrackGroupId  { get; set; }
    
    public bool? IsFrozen  { get; set; }

    public void AddDevice(IDevice device)
    {
        //TODO: should I get rid of the specific collections (stock, plugins, max4live) and put all devices in a single collection?

        if (device.Family.Type == DeviceType.Plugin)
            Plugins.Add(device as PluginDevice);
        if (device.Family.Type == DeviceType.Stock)
            StockDevices.Add(device as LiveDevice);
        else
            MaxForLiveDevices.Add(device as MaxForLiveDevice);
    }

    public void AddDevices(IReadOnlyList<IDevice> devices)
    {
        foreach (var device in devices)
            AddDevice(device);
    }
}