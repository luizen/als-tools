namespace AlsTools.Ui.Cli.CliOptions.Mappings;

public static class LocateOptionsMappingExtensions
{
    public static FilterContext MapToSpecification(this LocateOptions instance)
    {
        if (instance.IsEmpty)
            return FilterContext.Empty;

        var specification = new FilterContext();

        // General settings
        if (instance.LogicalOperator.HasValue)
            specification.FilterSettings.LogicalOperator = instance.LogicalOperator;        


        // Plugins
        if (instance.PluginNamesToLocate.HasValues())
            specification.PluginFilter.Names.AddRange(instance.PluginNamesToLocate!);

        if (instance.PluginFormatsToLocate.HasValues())
            specification.PluginFilter.Formats.AddRange(instance.PluginFormatsToLocate!);

        // Tracks
        if (instance.TrackUserNamesToLocate.HasValues())
            specification.TrackFilter.UserNames.AddRange(instance.TrackUserNamesToLocate!);

        if (instance.TrackEffectiveNamesToLocate.HasValues())
            specification.TrackFilter.EffectiveNames.AddRange(instance.TrackEffectiveNamesToLocate!);

        if (instance.TrackAnnotationsToLocate.HasValues())
            specification.TrackFilter.Annotations.AddRange(instance.TrackAnnotationsToLocate!);

        if (instance.TrackTypesToLocate.HasValues())
            specification.TrackFilter.Types.AddRange(instance.TrackTypesToLocate!);

        if (instance.TrackDelaysToLocate.HasValues())
            specification.TrackFilter.Delays.AddRange(instance.TrackDelaysToLocate!.Select(td => new TrackDelay() { Value = td }));

        if (instance.TrackContainsAnyMaxForLiveDevices.HasValue)
            specification.TrackFilter.ContainsAnyMaxForLiveDevices = instance.TrackContainsAnyMaxForLiveDevices;

        if (instance.TrackContainsAnyPlugins.HasValue)
            specification.TrackFilter.ContainsAnyPlugins = instance.TrackContainsAnyPlugins;

        if (instance.TrackContainsAnyStockDevices.HasValue)
            specification.TrackFilter.ContainsAnyStockDevices = instance.TrackContainsAnyStockDevices;

        if (instance.TrackContainsNumberOfMaxForLiveDevices.HasValue)
            specification.TrackFilter.ContainsNumberOfMaxForLiveDevices = instance.TrackContainsNumberOfMaxForLiveDevices;        

        if (instance.TrackContainsNumberOfPlugins.HasValue)
            specification.TrackFilter.ContainsNumberOfPlugins = instance.TrackContainsNumberOfPlugins;

        // Projects
        if (instance.ProjectNamesToLocate.HasValues())
            specification.LiveProjectFilter.Names.AddRange(instance.ProjectNamesToLocate!);

        if (instance.ProjectCreatorsToLocate.HasValues())
            specification.LiveProjectFilter.Creators.AddRange(instance.ProjectCreatorsToLocate!);

        if (instance.ProjectPathsToLocate.HasValues())
            specification.LiveProjectFilter.Paths.AddRange(instance.ProjectPathsToLocate!);

        if (instance.ProjectMajorVersionsToLocate.HasValues())
            specification.LiveProjectFilter.MajorVersions.AddRange(instance.ProjectMajorVersionsToLocate!);

        if (instance.ProjectMinorVersionsToLocate.HasValues())
            specification.LiveProjectFilter.MinorVersions.AddRange(instance.ProjectMinorVersionsToLocate!);

        if (instance.ProjectTemposToLocate.HasValues())
            specification.LiveProjectFilter.Tempos.AddRange(instance.ProjectTemposToLocate!);

        // Scenes
        if (instance.SceneNamesToLocate.HasValues())
            specification.SceneFilter.Names.AddRange(instance.SceneNamesToLocate!);

        if (instance.SceneTemposToLocate.HasValues())
            specification.SceneFilter.Tempos.AddRange(instance.SceneTemposToLocate!);

        if (instance.SceneAnnotationsToLocate.HasValues())
            specification.SceneFilter.Annotations.AddRange(instance.SceneAnnotationsToLocate!);

        // Stock devices
        if (instance.StockDeviceNamesToLocate.HasValues())
            specification.StockDeviceFilter.Names.AddRange(instance.StockDeviceNamesToLocate!);

        if (instance.StockDeviceUserNamesToLocate.HasValues())
            specification.StockDeviceFilter.UserNames.AddRange(instance.StockDeviceUserNamesToLocate!);
        
        if (instance.StockDeviceAnnotationsToLocate.HasValues())
            specification.StockDeviceFilter.Annotations.AddRange(instance.StockDeviceAnnotationsToLocate!);

        if (instance.StockDeviceSortsToLocate.HasValues())
            specification.StockDeviceFilter.Families.AddRange(instance.StockDeviceSortsToLocate!.Select(ds => new DeviceFamily(DeviceType.Stock, ds)));

        // Max For Live devices
        if (instance.MaxForLiveDeviceNamesToLocate.HasValues())
            specification.MaxForLiveDeviceFilter.Names.AddRange(instance.MaxForLiveDeviceNamesToLocate!);

        if (instance.MaxForLiveDeviceUserNamesToLocate.HasValues())
            specification.MaxForLiveDeviceFilter.UserNames.AddRange(instance.MaxForLiveDeviceUserNamesToLocate!);
        
        if (instance.MaxForLiveDeviceAnnotationsToLocate.HasValues())
            specification.MaxForLiveDeviceFilter.Annotations.AddRange(instance.MaxForLiveDeviceAnnotationsToLocate!);

        if (instance.MaxForLiveDeviceSortsToLocate.HasValues())
            specification.MaxForLiveDeviceFilter.Families.AddRange(instance.MaxForLiveDeviceSortsToLocate!.Select(ds => new DeviceFamily(DeviceType.MaxForLive, ds)));


        return specification;
    }
}