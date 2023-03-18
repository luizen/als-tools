namespace AlsTools.Infrastructure.Specifications;

public class TrackContainsAnyPluginsSpecification : ISpecification<LiveProject>
{
    public TrackContainsAnyPluginsSpecification()
    {
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Plugins.Any());
    }
}