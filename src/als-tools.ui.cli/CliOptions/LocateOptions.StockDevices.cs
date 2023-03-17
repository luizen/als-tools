using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Ui.Cli.CliOptions;
public partial class LocateOptions : CommonOptions
{
    [Option("stockdevice-names", Group = "locate options", Min = 1, HelpText = "The stock device names to locate projects by.")]
    public IReadOnlyCollection<string>? StockDeviceNamesToLocate { get; set; }

    [Option("stockdevice-user-names", Group = "locate options", Min = 1, HelpText = "The stock device user names to locate projects by.")]
    public IReadOnlyCollection<string>? StockDeviceUserNamesToLocate { get; set; }

    [Option("stockdevice-annotations", Group = "locate options", Min = 1, HelpText = "The stock device annotations to locate projects by.")]
    public IReadOnlyCollection<string>? StockDeviceAnnotationsToLocate { get; set; }

    [Option("stockdevice-sort", Group = "locate options", Min = 1, HelpText = "The stock device sorts to locate projects by.")]
    public IReadOnlyCollection<DeviceSort>? StockDeviceSortsToLocate { get; set; }
}