namespace AlsTools.Infrastructure.Repositories;

public partial class LiveProjectRavenRepository
{
    public async Task<IReadOnlyList<LiveProject>> Search(FilterContext filterContext)
    {
        using (var session = store.OpenAsyncSession())
        {
            bool useOrOperator = filterContext.FilterSettings.LogicalOperator == LogicOperators.Or;
            var query = session.Query<LiveProject>();

            var specifications = BuildSpecificationsFromFilterContext(filterContext);

            if (!specifications.Any())
                return Enumerable.Empty<LiveProject>().ToList();

            ISpecification<LiveProject> rootSpecification;

            if (specifications.Count == 1)
                rootSpecification = specifications[0];
            else if (useOrOperator)
                rootSpecification = new OrSpecification<LiveProject>(specifications.ToArray());
            else
                rootSpecification = new AndSpecification<LiveProject>(specifications.ToArray());

            List<LiveProject> results = await session.Query<LiveProject>().Where(rootSpecification.ToExpression()).ToListAsync();

            return results;
        }
    }

    private IList<ISpecification<LiveProject>> BuildSpecificationsFromFilterContext(FilterContext filterContext)
    {
        var specs = new List<ISpecification<LiveProject>>();

        if (filterContext.TrackFilter.UserNames.Any())
            specs.Add(new TrackNameSpecification(filterContext.TrackFilter.UserNames));

        if (filterContext.TrackFilter.EffectiveNames.Any())
            specs.Add(new TrackNameSpecification(filterContext.TrackFilter.EffectiveNames));

        if (filterContext.PluginFilter.Formats.Any())
            specs.Add(new PluginFormatSpecification(filterContext.PluginFilter.Formats.Select(f => f.ToString())));

        return specs;
    }
}