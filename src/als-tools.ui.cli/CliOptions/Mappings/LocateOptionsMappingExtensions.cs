using System.Linq.Expressions;
using AlsTools.Core.Entities;
using AlsTools.Core.Queries;

namespace AlsTools.Ui.Cli.CliOptions.Mappings;

public static class LocateOptionsMappingExtensions
{
    public static QuerySpecification MapToSpecification(this LocateOptions instance)
    {
        if (instance.IsEmpty) 
            return QuerySpecification.Empty;

        var specification = new QuerySpecification()
        {
            PluginQuery = new(),
            TrackQuery = new()
        };

        if (instance.PluginNamesToLocate != null && instance.PluginNamesToLocate.Any())
            specification.PluginQuery.Names.AddRange(instance.PluginNamesToLocate);

        if (instance.PluginFormatsToLocate != null && instance.PluginFormatsToLocate.Any())
            specification.PluginQuery.Formats.AddRange(instance.PluginFormatsToLocate);

        if (instance.TrackNamesToLocate != null && instance.TrackNamesToLocate.Any())
            specification.TrackQuery.UserNames.AddRange(instance.TrackNamesToLocate);

        return specification;
    }

    public static Expression<Func<LiveProject, bool>> MapToExpressionFilter(this LocateOptions instance)
    {
        throw new NotImplementedException();
        // Expression<Func<LiveProject, bool>> filter = _ => true;

        // if (instance.IsEmpty) 
        //     return filter;

        // if (instance.PluginNamesToLocate != null && instance.PluginNamesToLocate.Any())
        //     filter = filter.Parameters.and

        //     specification.PluginQuery.Names.AddRange(instance.PluginNamesToLocate);

        // if (instance.PluginFormatsToLocate != null && instance.PluginFormatsToLocate.Any())
        //     specification.PluginQuery.Formats.AddRange(instance.PluginFormatsToLocate);

        // return specification;
    }
}