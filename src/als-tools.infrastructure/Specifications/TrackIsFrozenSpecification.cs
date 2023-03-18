namespace AlsTools.Infrastructure.Specifications;

public class TrackIsFrozenSpecification : ISpecification<LiveProject>
{
    private readonly bool _isTrackFrozen;

    public TrackIsFrozenSpecification(bool isTrackFrozen)
    {
        this._isTrackFrozen = isTrackFrozen;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.IsFrozen == _isTrackFrozen);
    }
}