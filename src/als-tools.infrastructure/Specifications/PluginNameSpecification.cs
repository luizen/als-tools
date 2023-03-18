namespace AlsTools.Infrastructure.Specifications;

public class PluginNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> names;

    public PluginNameSpecification(IEnumerable<string> names)
    {
        this.names = names;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Plugins.Any(p => p.Name.In(names)));
    }
}