namespace AlsTools.Infrastructure.Specifications;

public class TrackContainsNumberOfPluginsSpecification : ISpecification<LiveProject>
{
    private readonly int _numberOfPlugins;

    public TrackContainsNumberOfPluginsSpecification(int numberOfPlugins)
    {
        this._numberOfPlugins = numberOfPlugins;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Plugins.Count == _numberOfPlugins);
    }
}