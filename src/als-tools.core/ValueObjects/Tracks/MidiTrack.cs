using AlsTools.Core.Entities;

namespace AlsTools.Core.ValueObjects.Tracks;

public class MidiTrack : BaseTrack, ITrack
{
    public MidiTrack(LiveProject liveProject) : base(TrackType.Midi, liveProject)
    {
    }
}
