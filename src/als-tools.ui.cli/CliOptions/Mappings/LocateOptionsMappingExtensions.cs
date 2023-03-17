using AlsTools.Core.Extensions;
using AlsTools.Core.Queries;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Ui.Cli.CliOptions.Mappings;

public static class LocateOptionsMappingExtensions
{
    public static QuerySpecification MapToSpecification(this LocateOptions instance)
    {
        if (instance.IsEmpty)
            return QuerySpecification.Empty;

        var specification = new QuerySpecification();

        // Plugins
        if (instance.PluginNamesToLocate.HasValues())
            specification.PluginQuery.Names.AddRange(instance.PluginNamesToLocate!);

        if (instance.PluginFormatsToLocate.HasValues())
            specification.PluginQuery.Formats.AddRange(instance.PluginFormatsToLocate!);

        // Tracks
        if (instance.TrackUserNamesToLocate.HasValues())
            specification.TrackQuery.UserNames.AddRange(instance.TrackUserNamesToLocate!);

        if (instance.TrackEffectiveNamesToLocate.HasValues())
            specification.TrackQuery.EffectiveNames.AddRange(instance.TrackEffectiveNamesToLocate!);

        if (instance.TrackAnnotationsToLocate.HasValues())
            specification.TrackQuery.Annotations.AddRange(instance.TrackAnnotationsToLocate!);

        if (instance.TrackTypesToLocate.HasValues())
            specification.TrackQuery.Types.AddRange(instance.TrackTypesToLocate!);

        if (instance.TrackDelaysToLocate.HasValues())
            specification.TrackQuery.Delays.AddRange(instance.TrackDelaysToLocate!.Select(td => new TrackDelay() { Value = td }));

        if (instance.TrackContainsAnyMaxForLiveDevices.HasValue)
            specification.TrackQuery.ContainsAnyMaxForLiveDevices = instance.TrackContainsAnyMaxForLiveDevices;

        if (instance.TrackContainsAnyPlugins.HasValue)
            specification.TrackQuery.ContainsAnyPlugins = instance.TrackContainsAnyPlugins;

        if (instance.TrackContainsAnyStockDevices.HasValue)
            specification.TrackQuery.ContainsAnyStockDevices = instance.TrackContainsAnyStockDevices;

        if (instance.TrackContainsNumberOfMaxForLiveDevices.HasValue)
            specification.TrackQuery.ContainsNumberOfMaxForLiveDevices = instance.TrackContainsNumberOfMaxForLiveDevices;        

        if (instance.TrackContainsNumberOfPlugins.HasValue)
            specification.TrackQuery.ContainsNumberOfPlugins = instance.TrackContainsNumberOfPlugins;

        // Projects
        if (instance.ProjectNamesToLocate.HasValues())
            specification.LiveProjectQuery.Names.AddRange(instance.ProjectNamesToLocate!);

        if (instance.ProjectCreatorsToLocate.HasValues())
            specification.LiveProjectQuery.Creators.AddRange(instance.ProjectCreatorsToLocate!);

        if (instance.ProjectPathsToLocate.HasValues())
            specification.LiveProjectQuery.Paths.AddRange(instance.ProjectPathsToLocate!);

        if (instance.ProjectMajorVersionsToLocate.HasValues())
            specification.LiveProjectQuery.MajorVersions.AddRange(instance.ProjectMajorVersionsToLocate!);

        if (instance.ProjectMinorVersionsToLocate.HasValues())
            specification.LiveProjectQuery.MinorVersions.AddRange(instance.ProjectMinorVersionsToLocate!);

        if (instance.ProjectTemposToLocate.HasValues())
            specification.LiveProjectQuery.Tempos.AddRange(instance.ProjectTemposToLocate!);

        // Scenes
        if (instance.SceneNamesToLocate.HasValues())
            specification.SceneQuery.Names.AddRange(instance.SceneNamesToLocate!);

        if (instance.SceneTemposToLocate.HasValues())
            specification.SceneQuery.Tempos.AddRange(instance.SceneTemposToLocate!);

        if (instance.SceneAnnotationsToLocate.HasValues())
            specification.SceneQuery.Annotations.AddRange(instance.SceneAnnotationsToLocate!);

        // Stock devices
        if (instance.StockDeviceNamesToLocate.HasValues())
            specification.StockDeviceQuery.Names.AddRange(instance.StockDeviceNamesToLocate!);

        if (instance.StockDeviceUserNamesToLocate.HasValues())
            specification.StockDeviceQuery.UserNames.AddRange(instance.StockDeviceUserNamesToLocate!);
        
        if (instance.StockDeviceAnnotationsToLocate.HasValues())
            specification.StockDeviceQuery.Annotations.AddRange(instance.StockDeviceAnnotationsToLocate!);

        if (instance.StockDeviceSortsToLocate.HasValues())
            specification.StockDeviceQuery.Families.AddRange(instance.StockDeviceSortsToLocate!.Select(ds => new DeviceFamily(DeviceType.Stock, ds)));

        // Max For Live devices
        if (instance.MaxForLiveDeviceNamesToLocate.HasValues())
            specification.MaxForLiveDeviceQuery.Names.AddRange(instance.MaxForLiveDeviceNamesToLocate!);

        if (instance.MaxForLiveDeviceUserNamesToLocate.HasValues())
            specification.MaxForLiveDeviceQuery.UserNames.AddRange(instance.MaxForLiveDeviceUserNamesToLocate!);
        
        if (instance.MaxForLiveDeviceAnnotationsToLocate.HasValues())
            specification.MaxForLiveDeviceQuery.Annotations.AddRange(instance.MaxForLiveDeviceAnnotationsToLocate!);

        if (instance.MaxForLiveDeviceSortsToLocate.HasValues())
            specification.MaxForLiveDeviceQuery.Families.AddRange(instance.MaxForLiveDeviceSortsToLocate!.Select(ds => new DeviceFamily(DeviceType.MaxForLive, ds)));


        return specification;
    }
}