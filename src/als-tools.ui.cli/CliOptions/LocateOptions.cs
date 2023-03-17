using AlsTools.Core.Extensions;

namespace AlsTools.Ui.Cli.CliOptions;

[Verb("locate", HelpText = "Locates projects containing any plugins (by their names and format) or any tracks (by their names) and other properties.")]
public partial class LocateOptions : CommonOptions
{
    public override bool IsEmpty => 
        !PluginNamesToLocate.HasValues() &&
        !PluginFormatsToLocate.HasValues() &&

        !TrackUserNamesToLocate.HasValues() &&
        !TrackEffectiveNamesToLocate.HasValues() &&
        !TrackAnnotationsToLocate.HasValues() &&
        !TrackIsFrozen.HasValue &&
        !TrackTypesToLocate.HasValues() &&
        !TrackDelaysToLocate.HasValues() &&
        !TrackContainsAnyMaxForLiveDevices.HasValue &&
        !TrackContainsAnyPlugins.HasValue &&
        !TrackContainsAnyStockDevices.HasValue &&
        !TrackContainsNumberOfMaxForLiveDevices.HasValue &&
        !TrackContainsNumberOfPlugins.HasValue &&
        !TrackContainsNumberOfStockDevices.HasValue &&

        !MaxForLiveDeviceNamesToLocate.HasValues() &&
        !MaxForLiveDeviceAnnotationsToLocate.HasValues() &&
        !MaxForLiveDeviceSortsToLocate.HasValues() &&
        !MaxForLiveDeviceUserNamesToLocate.HasValues() &&

        !SceneNamesToLocate.HasValues() &&
        !SceneTemposToLocate.HasValues() &&
        !SceneAnnotationsToLocate.HasValues() &&

        !StockDeviceNamesToLocate.HasValues() &&
        !StockDeviceUserNamesToLocate.HasValues() &&
        !StockDeviceAnnotationsToLocate.HasValues() &&
        !StockDeviceSortsToLocate.HasValues() &&

        !ProjectCreatorsToLocate.HasValues() &&
        !ProjectMajorVersionsToLocate.HasValues() &&
        !ProjectMinorVersionsToLocate.HasValues() &&
        !ProjectNamesToLocate.HasValues() &&
        !ProjectPathsToLocate.HasValues() &&
        !ProjectTemposToLocate.HasValues() &&
        
        !SceneNamesToLocate.HasValues() &&
        !SceneAnnotationsToLocate.HasValues() &&
        !SceneTemposToLocate.HasValues();
}