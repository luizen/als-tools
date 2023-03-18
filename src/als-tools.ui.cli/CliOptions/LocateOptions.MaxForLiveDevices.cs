namespace AlsTools.Ui.Cli.CliOptions;

public partial class LocateOptions : CommonOptions
{
    [Option("maxforlive-names", Group = "locate options", Min = 1, HelpText = "The Max For Live device names to locate projects by.")]
    public IReadOnlyCollection<string>? MaxForLiveDeviceNamesToLocate { get; set; }

    [Option("maxforlive-user-names", Group = "locate options", Min = 1, HelpText = "The Max For Live device user names to locate projects by.")]
    public IReadOnlyCollection<string>? MaxForLiveDeviceUserNamesToLocate { get; set; }

    [Option("maxforlive-annotations", Group = "locate options", Min = 1, HelpText = "The Max For Live device annotations to locate projects by.")]
    public IReadOnlyCollection<string>? MaxForLiveDeviceAnnotationsToLocate { get; set; }

    [Option("maxforlive-sort", Group = "locate options", Min = 1, HelpText = "The Max For Live device sorts to locate projects by.")]
    public IReadOnlyCollection<DeviceSort>? MaxForLiveDeviceSortsToLocate { get; set; }
}