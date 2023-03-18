using AlsTools.Core.ValueObjects;

namespace AlsTools.Ui.Cli.CliOptions;

public partial class LocateOptions : CommonOptions
{
    [Option("track-names", Group = "locate options", Min = 1, HelpText = "The track names to locate projects by.")]
    public IReadOnlyCollection<string>? TrackUserNamesToLocate { get; set; }

    [Option("track-effective-names", Group = "locate options", Min = 1, HelpText = "The track effective names to locate projects by.")]
    public IReadOnlyCollection<string>? TrackEffectiveNamesToLocate { get; set; }

    [Option("track-types", Group = "locate options", Min = 1, HelpText = "The track types to locate projects by.")]
    public IReadOnlyCollection<TrackType>? TrackTypesToLocate { get; set; }

    [Option("track-annotations", Group = "locate options", Min = 1, HelpText = "The track annotations to locate projects by.")]
    public IReadOnlyCollection<string>? TrackAnnotationsToLocate { get; set; }

    [Option("track-delays", Group = "locate options", Min = 1, HelpText = "The track delays to locate projects by.")]
    public IReadOnlyCollection<double>? TrackDelaysToLocate { get; set; }

    [Option("track-is-frozen", Group = "locate options", HelpText = "Whether to locate projects containing any frozen tracks.")]
    public bool? TrackIsFrozen { get; set; }

    [Option("track-contains-any-plugins", Group = "locate options", HelpText = "Whether to locate projects containing at least 1 plugin.")]
    public bool? TrackContainsAnyPlugins { get; set; }

    [Option("track-contains-num-plugins", Group = "locate options", HelpText = "Whether to locate projects containing an exact number of plugins.")]
    public int? TrackContainsNumberOfPlugins { get; set; }
    
    [Option("track-contains-any-stock-devices", Group = "locate options", HelpText = "Whether to locate projects containing at least 1 stock device.")]
    public bool? TrackContainsAnyStockDevices { get; set; }

    [Option("track-contains-num-stock-devices", Group = "locate options", HelpText = "Whether to locate projects containing an exact number of stock devices.")]
    public int? TrackContainsNumberOfStockDevices { get; set; }

    [Option("track-contains-any-maxforlive-devices", Group = "locate options", HelpText = "Whether to locate projects containing at least 1 Max For Live device.")]
    public bool? TrackContainsAnyMaxForLiveDevices { get; set; }

    [Option("track-contains-num-maxforlive-devices", Group = "locate options", HelpText = "Whether to locate projects containing an exact number of Max For Live devices.")]
    public bool? TrackContainsNumberOfMaxForLiveDevices { get; set; }
}