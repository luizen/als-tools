using System.Linq.Expressions;
using AlsTools.Core.Entities;
using AlsTools.Core.Specifications;
using Raven.Client.Documents.Linq;

namespace AlsTools.Infrastructure.Specifications;

public class PluginFormatSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> _formats;

    public PluginFormatSpecification(IEnumerable<string> formats)
    {
        _formats = formats;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Plugins.Any(p => p.Format.ToString().In(_formats)));
    }
}