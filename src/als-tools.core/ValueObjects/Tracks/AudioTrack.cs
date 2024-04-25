using AlsTools.Core.Entities;

namespace AlsTools.Core.ValueObjects.Tracks;

public class AudioTrack : BaseTrack, ITrack
{
    public AudioTrack(LiveProject liveProject) : base(TrackType.Audio, liveProject)
    {
    }
}
