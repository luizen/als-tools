namespace AlsTools.Core.Models.Tracks;

public class GroupTrack : BaseTrack
{
    public GroupTrack() : this(new List<BaseTrack>())
    {
    }

    public GroupTrack(IList<BaseTrack> childrenTracks) : base(TrackType.Group)
    {
        ChildrenTracks = childrenTracks;
    }

    /// <summary>
    /// The children tracks that this group track groups under it.
    /// </summary>
    public IList<BaseTrack> ChildrenTracks { get; set; } //TODO: this shouldn`t be allowed to be set from external...
}