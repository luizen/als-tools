namespace AlsTools.Infrastructure.Specifications;

public class PluginNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> _pluginNames;

    public PluginNameSpecification(IEnumerable<string> pluginNames)
    {
        _pluginNames = pluginNames;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Plugins.Any(p => p.Name.In(_pluginNames)));
    }
}