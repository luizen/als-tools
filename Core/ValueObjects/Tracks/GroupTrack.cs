namespace AlsTools.Core.ValueObjects.Tracks;

public class GroupTrack : BaseTrack, ITrack
{
    public GroupTrack() : this(new List<ITrack>())
    {
    }

    public GroupTrack(IList<ITrack> childrenTracks) : base(TrackType.Group)
    {
        ChildrenTracks = childrenTracks;
    }

    /// <summary>
    /// The children tracks that this group track groups under it.
    /// </summary>
    public IList<ITrack> ChildrenTracks { get; set; } //TODO: this shouldn`t be allowed to be set from external...
}    
