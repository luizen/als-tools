using AlsTools.Core.Queries;

namespace AlsTools.Ui.Cli.CliOptions.Mappings;

public static class LocateOptionsMappingExtensions
{
    public static QuerySpecification MapToSpecification(this LocateOptions instance)
    {
        if (instance.PluginNamesToLocate == null || !instance.PluginNamesToLocate.Any())
            return QuerySpecification.Empty;

        var specification = new QuerySpecification()
        {
            PluginQuery = new()
        };

        specification.PluginQuery.Names.AddRange(instance.PluginNamesToLocate);
        specification.PluginQuery.Formats.AddRange(instance.PluginFormatsToLocate);

        return specification;
    }
}