namespace AlsTools.Ui.Cli.CliOptions;

[Verb("locate", HelpText = "Locates projects containing any plugins (by their names and format) or any tracks (by their names) and other properties.")]
public partial class LocateOptions : CommonOptions
{
    [Option("logical-operator", Group = "locate options", HelpText = "The logical operator, either OR or AND, to be used when combining all filters.", Default = LogicOperators.And)]
    public LogicOperators? LogicalOperator { get; set; }

    [Option("compact-output", Group = "locate options", HelpText = "Whether to display compact output containing only Project Name and Path", Default = false)]
    public bool CompactOutput { get; set; }

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