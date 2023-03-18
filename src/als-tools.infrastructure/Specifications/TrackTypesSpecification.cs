namespace AlsTools.Infrastructure.Specifications;

public class TrackTypesSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<TrackType> types;

    public TrackTypesSpecification(IEnumerable<TrackType> types)
    {
        this.types = types;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Type.In(types));
    }
}