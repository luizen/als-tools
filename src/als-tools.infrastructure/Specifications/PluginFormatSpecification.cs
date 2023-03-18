namespace AlsTools.Infrastructure.Specifications;

public class PluginFormatSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> formats;

    public PluginFormatSpecification(IEnumerable<string> formats)
    {
        this.formats = formats;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Plugins.Any(p => p.Format.ToString().In(formats)));
    }
}