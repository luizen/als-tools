using AlsTools.Core.Entities;
using AlsTools.Core.Queries;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Linq.PredicateBuilder;
using System.Linq.Expressions;

namespace AlsTools.Infrastructure.Repositories;

public partial class LiveProjectRavenRepository
{

    public async Task<IReadOnlyList<LiveProject>> Search(QuerySpecification specification)
    {
        return await DoSearch9(specification);
    }

    // WORKS!!
    private async Task<IReadOnlyList<LiveProject>> DoSearch9(QuerySpecification specification)
    {
        using var session = store.OpenAsyncSession();
        string[] trackNames = specification.TrackQuery != null && specification.TrackQuery.UserNames.Any() ? specification.TrackQuery.UserNames.ToArray() : Array.Empty<string>();
        string[] pluginFormats = specification.PluginQuery != null && specification.PluginQuery.Formats.Any() ? specification.PluginQuery.Formats.Select(f => f.ToString()).ToArray() : Array.Empty<string>();
        bool useOrOperator = true; // Set this value based on user input
        var specs = new List<ISpecification<LiveProject>>();
        var query = session.Query<LiveProject>();

        if (trackNames.Length > 0)
            specs.Add(new TrackNameSpecification(trackNames));

        if (pluginFormats.Length > 0)
            specs.Add(new PluginFormatSpecification(pluginFormats));
        
        if (specs.Count > 0)
        {
            ISpecification<LiveProject> spec;

            if (specs.Count == 1)
                spec = specs[0];
            else if (useOrOperator)
                spec = new OrSpecification<LiveProject>(specs.ToArray());
            else
                spec = new AndSpecification<LiveProject>(specs.ToArray());

            List<LiveProject> results = await session.Query<LiveProject>().Where(spec.ToExpression()).ToListAsync();

            return results;
        }

        return Enumerable.Empty<LiveProject>().ToList();
    }

    // WORKS!
    private async Task<IReadOnlyList<LiveProject>> DoSearch8(QuerySpecification specification)
    {
        using var session = store.OpenAsyncSession();
        var query = session.Advanced.AsyncDocumentQuery<LiveProject>();

        string[] trackNames = specification.TrackQuery != null && specification.TrackQuery.UserNames.Any() ? specification.TrackQuery.UserNames.ToArray() : Array.Empty<string>();
        if (trackNames.Length > 0)
            query = query.WhereIn("Tracks.UserName", trackNames);

        string[] pluginNames = specification.PluginQuery != null && specification.PluginQuery.Names.Any() ? specification.PluginQuery.Names.ToArray() : Array.Empty<string>();
        if (pluginNames.Length > 0)
            query = query.OrElse().WhereIn("Tracks.Plugins.Name", pluginNames);

        string[] pluginFormats = specification.PluginQuery != null && specification.PluginQuery.Formats.Any() ? specification.PluginQuery.Formats.Select(f => f.ToString()).ToArray() : Array.Empty<string>();
        if (pluginFormats.Length > 0)
            query = query.OrElse().WhereIn("Tracks.Plugins.Format", pluginFormats);

        List<LiveProject> results = await query.ToListAsync();

        return results;
    }

    // WORKS!!!
    private async Task<IReadOnlyList<LiveProject>> DoSearch7(QuerySpecification specification)
    {
        using var session = store.OpenAsyncSession();
        var query = session.Query<LiveProject>();

        string[] trackNames = specification.TrackQuery != null && specification.TrackQuery.UserNames.Any() ? specification.TrackQuery.UserNames.ToArray() : Array.Empty<string>();

        string[] pluginNames = specification.PluginQuery != null && specification.PluginQuery.Names.Any() ? specification.PluginQuery.Names.ToArray() : Array.Empty<string>();

        query = query.Where(lp => lp.Tracks.Any(track => track.UserName.In(trackNames)) ||
                                  lp.Tracks.Any(track => track.Plugins.Any(plugin => plugin.Name.In(pluginNames))));

        List<LiveProject> results = await query.ToListAsync();

        return results;
    }

    // WORKS!
    private async Task<IReadOnlyList<LiveProject>> DoSearch6(QuerySpecification specification)
    {
        using var session = store.OpenAsyncSession();
        var query = session.Query<LiveProject>();

        string[] trackNames = specification.TrackQuery != null && specification.TrackQuery.UserNames.Any() ? specification.TrackQuery.UserNames.ToArray() : Array.Empty<string>();
        string trackName = trackNames[0];

        if (trackNames.Any())
        {
            // query = query.Where(lp => lp.Tracks.Any(t => trackNames.Contains(t.UserName)));
            query = query.Where(lp => lp.Tracks.Any(t => t.UserName.In(trackNames)));
            // query = query.Where(lp => lp.Tracks.Any(t => t.UserName == trackName));
        }

        List<LiveProject> results = await query.ToListAsync();

        return results;
    }

    // WORKS!
    private async Task<IReadOnlyList<LiveProject>> DoSearch5(QuerySpecification specification)
    {
        using var session = store.OpenAsyncSession();
        var query = session.Query<LiveProject>();

        string[] trackNames = specification.TrackQuery != null && specification.TrackQuery.UserNames.Any() ? specification.TrackQuery.UserNames.ToArray() : Array.Empty<string>();
        string trackName = trackNames[0];

        if (trackNames.Any())
        {
            // query = query.Where(lp => lp.Tracks.Any(t => trackNames.Contains(t.UserName)));
            query = query.Where(lp => lp.Tracks.Any(t => t.UserName == trackName));
        }

        List<LiveProject> results = await query.ToListAsync();

        return results;
    }

    // WORKS!
    private async Task<IReadOnlyList<LiveProject>> DoSearch4(QuerySpecification specification)
    {
        using var session = store.OpenAsyncSession();
        List<LiveProject> results = await session
            .Query<LiveProject>()
            .Where(lp => lp.Tracks.Any(t => t.Plugins.Any(p => p.Name == "FreeAMP")))
            .ToListAsync();

        return results;
    }

    // WORKS!
    private async Task<IReadOnlyList<LiveProject>> DoSearch3(QuerySpecification specification)
    {
        using var session = store.OpenAsyncSession();
        List<LiveProject> results = await session
            .Query<LiveProject>()
            .Where(lp => lp.Tracks.Any(t => t.Plugins.Any(p => p.Format == Core.ValueObjects.PluginFormat.AU)))
            .ToListAsync();

        return results;
    }

    // DOES NOT WORK
    private async Task<IReadOnlyList<LiveProject>> DoSearch2(QuerySpecification specification)
    {
        using var session = store.OpenAsyncSession();
        var query = session.Query<LiveProject>();

        string[] trackNames = specification.TrackQuery != null && specification.TrackQuery.UserNames.Any() ? specification.TrackQuery.UserNames.ToArray() : Array.Empty<string>();
        string trackName = trackNames[0];

        if (trackNames.Any())
        {
            // query = query.Where(lp => lp.Tracks.Any(t => trackNames.Contains(t.UserName)));
            query = query.Where(lp => lp.Tracks.Any(t => t.UserName == trackName));

        }

        string[] pluginNames = specification.PluginQuery != null && specification.PluginQuery.Names.Any() ? specification.PluginQuery.Names.ToArray() : Array.Empty<string>();
        string pluginName = pluginNames[0];

        if (pluginNames.Any())
        {
            // if (useAndOperator && !string.IsNullOrEmpty(trackName))
            // {
            //     query = query.Where(lp => lp.Tracks.Any(t => t.Plugins.Any(p => p.Format == pluginDeviceFormat)));
            // }
            // else
            // {
            //     query = query.Where(lp => lp.Tracks.Any(t => t.Plugins.Any(p => p.Format == pluginDeviceFormat)));
            // }
            // query = query.Where(lp => lp.Tracks.Any(track => track.Plugins.Any(plugin => pluginNames.Contains(plugin.Name))));
            query = query.Where(lp => lp.Tracks.Any(track => track.Plugins.Any(plugin => plugin.Name == pluginName)));
        }

        return await query.ToListAsync();
    }

    // DOES NOT WORK
    private async Task<IReadOnlyList<LiveProject>> DoSearch1(QuerySpecification specification)
    {
        using (var session = store.OpenAsyncSession())
        {
            var query = session.Advanced.AsyncDocumentQuery<LiveProject>();

            var filteredQuery = AddFiltersToQuery(query, specification);

            var results = await filteredQuery.ToListAsync();

            return results;
        }
    }

    private IQueryable<LiveProject> AddFiltersToQuery(IAsyncDocumentQuery<LiveProject> query, QuerySpecification specification)
    {

        IEnumerable<string> trackNames = specification.PluginQuery != null && specification.PluginQuery.Names.Any() ? specification.PluginQuery.Names : Enumerable.Empty<string>();
        var hasPluginName = specification.PluginQuery != null && specification.PluginQuery.Names.Any();

        var resultQuery = query.ToQueryable().Build(_ => _
            .Conditional(trackNames.Any()).Where(lp => lp.Tracks.Any(track => trackNames.Contains(track.UserName)))
        );

        return resultQuery;
    }

}



public interface ISpecification<T>
{
    Expression<Func<T, bool>> ToExpression();
}

public class TrackNameSpecification : ISpecification<LiveProject>
{
    private readonly string[] _trackNames;

    public TrackNameSpecification(string[] trackNames)
    {
        _trackNames = trackNames;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.UserName.In(_trackNames));
    }
}

public class PluginFormatSpecification : ISpecification<LiveProject>
{
    private readonly string[] _formats;

    public PluginFormatSpecification(string[] formats)
    {
        _formats = formats;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Plugins.Any(p => p.Format.ToString().In(_formats)));
    }
}

public abstract class CompositeSpecification<T> : ISpecification<T>
{
    protected readonly ISpecification<T>[] _specifications;

    protected CompositeSpecification(params ISpecification<T>[] specifications)
    {
        _specifications = specifications;
    }

    public abstract Expression<Func<T, bool>> ToExpression();
}

public class OrSpecification<T> : CompositeSpecification<T>
{
    public OrSpecification(params ISpecification<T>[] specifications) : base(specifications) { }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var parameter = Expression.Parameter(typeof(T));
        var body = _specifications.Select(s => s.ToExpression().Body.ReplaceParameter(s.ToExpression().Parameters[0], parameter)).Aggregate(Expression.OrElse);
        return Expression.Lambda<Func<T,bool>>(body, parameter);
    }
}

public class AndSpecification<T> : CompositeSpecification<T>
{
    public AndSpecification(params ISpecification<T>[] specifications) : base(specifications) { }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var parameter = Expression.Parameter(typeof(T));
        var body = _specifications.Select(s => s.ToExpression().Body.ReplaceParameter(s.ToExpression().Parameters[0], parameter)).Aggregate(Expression.AndAlso);
        return Expression.Lambda<Func<T,bool>>(body, parameter);
    }
}

public static class ExpressionExtensions
{
    public static Expression ReplaceParameter(this Expression expression, ParameterExpression source, ParameterExpression target)
    {
        return new ParameterReplacer { Source = source, Target = target }.Visit(expression);
    }

    private class ParameterReplacer : ExpressionVisitor
    {
        public ParameterExpression Source { get; set; }

        public ParameterExpression Target { get; set; }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == Source ? Target : base.VisitParameter(node);
        }
    }
}