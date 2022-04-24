using System.Collections.Generic;
using AlsTools.Core.Entities.Devices;

namespace AlsTools.Core.Entities.Tracks
{
    public interface ITrack
    {
        string Name { get; set; }

        TrackType Type { get; set; }

        SortedDictionary<string, LiveDevice> Devices { get; }

        SortedDictionary<string, PluginDevice> Plugins { get; }

        void AddDevice(IDevice device);
    }
}