using AlsTools.Infrastructure.Indexes;

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
                rootSpecification = specifications.First();
            else if (useOrOperator)
                rootSpecification = new OrSpecification<LiveProject>(specifications.ToArray());
            else
                rootSpecification = new AndSpecification<LiveProject>(specifications.ToArray());

            List<LiveProject> results = await session.Query<LiveProject>().Where(rootSpecification.ToExpression()).ToListAsync();

            return results;
        }
    }

    // public async Task<IReadOnlyList<LiveProject>> Search(FilterContext filterContext)
    // {
    //     string[] pluginNames = filterContext.PluginFilter.Names.ToArray();
    //     string query = $"{pluginNames[0]}*";

    //     using var session = store.OpenAsyncSession();
    //     List<LiveProject> lps = await session
    //             .Query<LiveProjects_ByPluginNames.PluginNameResult, LiveProjects_ByPluginNames>()
    //             .Search(p => p.PluginName, query)
    //             .As<LiveProject>()
    //             .ToListAsync();

    //     return lps;
    // }

    // FUNCIONA!
    // public async Task<IReadOnlyList<LiveProject>> Search_PluginName_StartsWith(FilterContext filterContext)
    // {
    //     string[] pluginNames = filterContext.PluginFilter.Names.ToArray();
    //     string query = $"{pluginNames[0]}*";

    //     using var session = store.OpenAsyncSession();
    //     List<LiveProject> lps = await session
    //             .Query<LiveProjects_ByPluginNames.PluginNameResult, LiveProjects_ByPluginNames>()
    //             .Search(p => p.Name, query)
    //             .As<LiveProject>()
    //             .ToListAsync();

    //     return lps;
    // }


    // FUNCIONA (Like)
    // public async Task<IReadOnlyList<LiveProject>> Search_EndsWith(FilterContext filterContext)
    // {
    //     string[] projectNames = filterContext.LiveProjectFilter.Names.ToArray();
    //     string query = $"*{projectNames[0]}";

    //     using var session = store.OpenAsyncSession();
    //     List<LiveProject> lps = await session
    //             .Query<LiveProject>()
    //             .Search(proj => proj.Name, query)
    //             .ToListAsync();

    //     return lps;
    // }


    // FUNCIONA (WhereStartsWith)
    // public async Task<IReadOnlyList<LiveProject>> Search(FilterContext filterContext)
    // {
    //     using var session = store.OpenAsyncSession();
    //     var query = session.Advanced.AsyncDocumentQuery<LiveProject>();

    //     string[] projectNames = filterContext.LiveProjectFilter.Names.ToArray();
    //     if (projectNames.Length > 0)
    //         query = query.WhereStartsWith<string>(lp => lp.Name, projectNames[0]);

    //     List<LiveProject> results = await query.ToListAsync();

    //     return results;
    // }

        
    // public async Task<IReadOnlyList<LiveProject>> Search(FilterContext filterContext)
    // {
    //     using var session = store.OpenAsyncSession();

    //     // IList<LiveProject> allProjects = await session.Query<LiveProject>().ToListAsync();

    //     // IList<LiveProject> projects = session
    //     //     .Query<LiveProject>()
    //     //     .Search(lp => lp.Name, "John Steve")
    //     //     .ToList();

    //     var lp = await session
    //     .Query<LiveProjects_FullTextSearch.Result, LiveProjects_FullTextSearch>()
    //     .Search(x => x.Query, "NEOLD")
    //     .As<LiveProject>()
    //     .ToListAsync();
    
    //     return null;
    // }



    // FUNCIONA!
    // public async Task<IReadOnlyList<LiveProject>> FullTextSearch(FilterContext filterContext)
    // {
    //     using var session = store.OpenAsyncSession();

    //     // IList<LiveProject> allProjects = await session.Query<LiveProject>().ToListAsync();

    //     // IList<LiveProject> projects = session
    //     //     .Query<LiveProject>()
    //     //     .Search(lp => lp.Name, "John Steve")
    //     //     .ToList();

    //     var lp = await session
    //     .Query<LiveProjects_FullTextSearch.Result, LiveProjects_FullTextSearch>()
    //     .Search(x => x.Query, "NEOLD")
    //     .As<LiveProject>()
    //     .ToListAsync();
    
    //     return null;
    // }
    
    // FUNCIONA!
    // public async Task<IReadOnlyList<LiveProject>> SearchWhereStartsWith(FilterContext filterContext)
    // {
    //     using var session = store.OpenAsyncSession();
    //     var query = session.Advanced.AsyncDocumentQuery<LiveProject>();

    //     query = query.WhereStartsWith("Tracks.Plugins.Name", "NEOLD");

    //     List<LiveProject> results = await query.ToListAsync();

    //     return results;
    // }

    private IList<ISpecification<LiveProject>> BuildSpecificationsFromFilterContext(FilterContext filterContext)
    {
        var specs = new List<ISpecification<LiveProject>>();

        // Track filters
        if (filterContext.TrackFilter.UserNames.Any())
            specs.Add(new TrackNameSpecification(filterContext.TrackFilter.UserNames, filterContext.FilterSettings.TextMatchingOption));

        if (filterContext.TrackFilter.EffectiveNames.Any())
            specs.Add(new TrackEffectiveNameSpecification(filterContext.TrackFilter.EffectiveNames));

        if (filterContext.TrackFilter.Annotations.Any())
            specs.Add(new TrackAnnotationSpecification(filterContext.TrackFilter.Annotations));

        if (filterContext.TrackFilter.Types.Any())
            specs.Add(new TrackTypesSpecification(filterContext.TrackFilter.Types));

        if (filterContext.TrackFilter.Delays.Any())
            specs.Add(new TrackDelaySpecification(filterContext.TrackFilter.Delays));

        if (filterContext.TrackFilter.IsFrozen.HasValue)
            specs.Add(new TrackIsFrozenSpecification(filterContext.TrackFilter.IsFrozen.Value));

        if (filterContext.TrackFilter.ContainsAnyPlugins.HasValue)
            specs.Add(new TrackContainsAnyPluginsSpecification());

        if (filterContext.TrackFilter.ContainsAnyStockDevices.HasValue)
            specs.Add(new TrackContainsAnyStockDevicesSpecification());

        if (filterContext.TrackFilter.ContainsAnyMaxForLiveDevices.HasValue)
            specs.Add(new TrackContainsAnyMaxForLiveDevicesSpecification());

        if (filterContext.TrackFilter.ContainsNumberOfPlugins.HasValue)
            specs.Add(new TrackContainsNumberOfPluginsSpecification(filterContext.TrackFilter.ContainsNumberOfPlugins.Value));

        if (filterContext.TrackFilter.ContainsNumberOfMaxForLiveDevices.HasValue)
            specs.Add(new TrackContainsNumberOfMaxForLiveDevicesSpecification(filterContext.TrackFilter.ContainsNumberOfMaxForLiveDevices.Value));

        // Plugin filters
        if (filterContext.PluginFilter.Formats.Any())
            specs.Add(new PluginFormatSpecification(filterContext.PluginFilter.Formats.Select(f => f.ToString())));

        if (filterContext.PluginFilter.Names.Any())
            specs.Add(new PluginNameSpecification(filterContext.PluginFilter.Names, filterContext.FilterSettings.TextMatchingOption));

        if (filterContext.PluginFilter.Sorts.Any())
            specs.Add(new PluginSortSpecification(filterContext.PluginFilter.Sorts));

        // Scene filters
        if (filterContext.SceneFilter.Names.Any())
            specs.Add(new SceneNameSpecification(filterContext.SceneFilter.Names));

        if (filterContext.SceneFilter.Annotations.Any())
            specs.Add(new SceneAnnotationSpecification(filterContext.SceneFilter.Annotations));

        if (filterContext.SceneFilter.Tempos.Any())
            specs.Add(new SceneTemposSpecification(filterContext.SceneFilter.Tempos));


        // Stock devices
        if (filterContext.StockDeviceFilter.Names.Any())
            specs.Add(new StockDeviceNameSpecification(filterContext.StockDeviceFilter.Names));

        if (filterContext.StockDeviceFilter.UserNames.Any())
            specs.Add(new StockDeviceUserNamesSpecification(filterContext.StockDeviceFilter.UserNames));

        if (filterContext.StockDeviceFilter.Annotations.Any())
            specs.Add(new StockDeviceAnnotationSpecification(filterContext.StockDeviceFilter.Annotations));

        if (filterContext.StockDeviceFilter.Families.Any())
            specs.Add(new StockDeviceFamiliesSpecification(filterContext.StockDeviceFilter.Families));

        // Max For Live devices
        if (filterContext.MaxForLiveDeviceFilter.Names.Any())
            specs.Add(new MaxForLiveDeviceNameSpecification(filterContext.MaxForLiveDeviceFilter.Names));

        if (filterContext.MaxForLiveDeviceFilter.UserNames.Any())
            specs.Add(new MaxForLiveDeviceUserNamesSpecification(filterContext.MaxForLiveDeviceFilter.UserNames));

        if (filterContext.MaxForLiveDeviceFilter.Annotations.Any())
            specs.Add(new MaxForLiveDeviceAnnotationSpecification(filterContext.MaxForLiveDeviceFilter.Annotations));

        if (filterContext.MaxForLiveDeviceFilter.Families.Any())
            specs.Add(new MaxForLiveDeviceFamiliesSpecification(filterContext.MaxForLiveDeviceFilter.Families));

        // Live projects
        if (filterContext.LiveProjectFilter.Names.Any())
            specs.Add(new LiveProjectNamesSpecification(filterContext.LiveProjectFilter.Names, filterContext.FilterSettings.TextMatchingOption));

        if (filterContext.LiveProjectFilter.Paths.Any())
            specs.Add(new LiveProjectPathsSpecification(filterContext.LiveProjectFilter.Paths));

        if (filterContext.LiveProjectFilter.Tempos.Any())
            specs.Add(new LiveProjectTemposSpecification(filterContext.LiveProjectFilter.Tempos));

        if (filterContext.LiveProjectFilter.Creators.Any())
            specs.Add(new LiveProjectCreatorsSpecification(filterContext.LiveProjectFilter.Creators));

        if (filterContext.LiveProjectFilter.MajorVersions.Any())
            specs.Add(new LiveProjectMajorVersionsSpecification(filterContext.LiveProjectFilter.MajorVersions));

        if (filterContext.LiveProjectFilter.MinorVersions.Any())
            specs.Add(new LiveProjectMinorVersionsSpecification(filterContext.LiveProjectFilter.MinorVersions));


        return specs;
    }
}