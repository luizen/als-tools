namespace AlsTools.Infrastructure.Specifications;

public class PluginSortSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<DeviceSort> _pluginSorts;
    private readonly IEnumerable<string> _pluginSortTexts;

    public PluginSortSpecification(IEnumerable<DeviceSort> pluginSorts)
    {
        _pluginSorts = pluginSorts;
        _pluginSortTexts = pluginSorts.Select(ps => ps.ToString());
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Plugins.Any(p => p.Family.Sort.ToString().In(_pluginSortTexts)));
    }
}