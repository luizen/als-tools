namespace AlsTools.Core.Models.Tracks;

public class GroupTrack : BaseTrack
{
    public GroupTrack() : base(TrackType.Group)
    {
    }

    /// <summary>
    /// The children tracks that this group track groups under it.
    /// </summary>
    public virtual ICollection<BaseTrack> ChildrenTracks { get; set; } = [];
}