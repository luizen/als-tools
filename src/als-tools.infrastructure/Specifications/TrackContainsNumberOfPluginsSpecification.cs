namespace AlsTools.Infrastructure.Specifications;

public class TrackContainsNumberOfPluginsSpecification : ISpecification<LiveProject>
{
    private readonly int numberOfPlugins;

    public TrackContainsNumberOfPluginsSpecification(int numberOfPlugins)
    {
        this.numberOfPlugins = numberOfPlugins;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Plugins.Count == numberOfPlugins);
    }
}