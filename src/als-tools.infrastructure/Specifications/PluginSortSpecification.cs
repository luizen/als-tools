namespace AlsTools.Infrastructure.Specifications;

public class PluginSortSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<DeviceSort> sorts;
    private readonly IEnumerable<string> sortTexts;

    public PluginSortSpecification(IEnumerable<DeviceSort> sorts)
    {
        this.sorts = sorts;
        sortTexts = sorts.Select(ps => ps.ToString());
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Plugins.Any(p => p.Family.Sort.ToString().In(sortTexts)));
    }
}