using System.Collections.Generic;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.ValueObjects.Tracks
{
    public abstract class BaseTrack : ITrack
    {
        public BaseTrack(TrackType type)
        {
            Devices = new SortedDictionary<string, LiveDevice>();
            Plugins = new SortedDictionary<string, PluginDevice>();
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

        public SortedDictionary<string, LiveDevice> Devices { get; protected set; }

        public SortedDictionary<string, PluginDevice> Plugins { get; protected set; }

        public string Annotation { get; set; }

        public GroupTrack ParentGroupTrack { get; set; }

        public bool IsPartOfGroup => ParentGroupTrack != null;

        public TrackDelay TrackDelay { get; set; }
       
        public int? TrackGroupId  { get; set; }
        
        public bool? IsFrozen  { get; set; }

        public void AddDevice(IDevice device)
        {
            if (device.Type == DeviceType.Plugin)
            {
                if (!Plugins.ContainsKey(device.Name))
                    Plugins.Add(device.Name, device as PluginDevice);
            }
            else
            {
                if (!Devices.ContainsKey(device.Name))
                    Devices.Add(device.Name, device as LiveDevice);
            }
        }
    }
}