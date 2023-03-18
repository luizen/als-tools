namespace AlsTools.Infrastructure.Specifications;

public class TrackIsFrozenSpecification : ISpecification<LiveProject>
{
    private readonly bool isTrackFrozen;

    public TrackIsFrozenSpecification(bool isTrackFrozen)
    {
        this.isTrackFrozen = isTrackFrozen;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.IsFrozen == isTrackFrozen);
    }
}