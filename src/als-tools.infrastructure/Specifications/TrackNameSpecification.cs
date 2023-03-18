namespace AlsTools.Infrastructure.Specifications;

public class TrackNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> _trackNames;

    public TrackNameSpecification(IEnumerable<string> trackNames)
    {
        _trackNames = trackNames;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.UserName.In(_trackNames));
    }
}