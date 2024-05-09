using AlsTools.Core.ValueObjects.Tracks;

namespace AlsTools.Core.ValueObjects.Devices;

public interface IDevice
{
    int Id { get; set; }

    public int TrackId { get; set; }
    public ITrack Track { get; set; }

    string Name { get; set; }

    DeviceFamily Family { get; }
}
