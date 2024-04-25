using AlsTools.Core.Entities;

namespace AlsTools.Core.ValueObjects.Tracks;

public class ReturnTrack : BaseTrack, ITrack
{
    public ReturnTrack(LiveProject liveProject) : base(TrackType.Return, liveProject)
    {
    }
}
