using AlsTools.Core.Entities;

namespace AlsTools.Core.ValueObjects.Tracks;

public class MasterTrack : BaseTrack, ITrack
{
    public MasterTrack(LiveProject liveProject) : base(TrackType.Master, liveProject)
    {
    }
}