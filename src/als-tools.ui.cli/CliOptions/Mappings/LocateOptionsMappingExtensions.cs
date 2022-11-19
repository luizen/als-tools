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
            PluginQuery = new()
        };

        if (instance.PluginNamesToLocate != null && instance.PluginNamesToLocate.Any())
            specification.PluginQuery.Names.AddRange(instance.PluginNamesToLocate);

        if (instance.PluginFormatsToLocate != null && instance.PluginFormatsToLocate.Any())
            specification.PluginQuery.Formats.AddRange(instance.PluginFormatsToLocate);

        return specification;
    }
}