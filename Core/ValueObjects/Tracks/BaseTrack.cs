using System.Collections.Generic;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.ValueObjects.Tracks
{
    public abstract class BaseTrack : ITrack
    {
        public BaseTrack(TrackType type)
        {
            Devices = new List<LiveDevice>();
            Plugins = new List<PluginDevice>();
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

        public IList<LiveDevice> Devices { get; protected set; }

        public IList<PluginDevice> Plugins { get; protected set; }

        public string Annotation { get; set; }

        public GroupTrack ParentGroupTrack { get; set; }

        public bool IsPartOfGroup => ParentGroupTrack != null;

        public TrackDelay TrackDelay { get; set; }
       
        public int? TrackGroupId  { get; set; }
        
        public bool? IsFrozen  { get; set; }

        public void AddDevice(IDevice device)
        {
            if (device.Type == DeviceType.Plugin)
                Plugins.Add(device as PluginDevice);
            else
                Devices.Add(device as LiveDevice);
        }
    }
}