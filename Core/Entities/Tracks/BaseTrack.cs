using System.Collections.Generic;
using AlsTools.Core.Entities.Devices;

namespace AlsTools.Core.Entities.Tracks
{
    public abstract class BaseTrack : ITrack
    {
        public BaseTrack(TrackType type)
        {
            Devices = new SortedDictionary<string, LiveDevice>();
            Plugins = new SortedDictionary<string, PluginDevice>();
            Type = type;
        }

        /// <summary>
        /// Track name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether the track is an audio, midi or return track
        /// </summary>
        public TrackType Type { get; set; }

        /// <summary>
        /// All Live devices used in this track
        /// </summary>
        public SortedDictionary<string, LiveDevice> Devices { get; protected set; }
        
        /// <summary>
        /// All plugins used in this track
        /// </summary>
        public SortedDictionary<string, PluginDevice> Plugins { get; protected set; }

        /// <summary>
        /// Adds a device to either the <see href="Devices" /> or <see href="Plugins" />
        /// list, only if it does not exist yet.
        /// </summary>
        /// <param name="device">The device object</param>
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