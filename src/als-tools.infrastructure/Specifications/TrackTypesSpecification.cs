namespace AlsTools.Infrastructure.Specifications;

public class TrackTypesSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<TrackType> _trackTypes;

    public TrackTypesSpecification(IEnumerable<TrackType> trackTypes)
    {
        _trackTypes = trackTypes;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Type.In(_trackTypes));
    }
}