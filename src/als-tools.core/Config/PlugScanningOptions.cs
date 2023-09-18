using AlsTools.Core.ValueObjects;

namespace AlsTools.Core.Config;

/// <summary>
/// PlugScanning options, to be configured via appsettings.json file
/// </summary>
public class PlugScanningOptions
{
    /// <summary>
    /// Whether to skip any plugins or not
    /// </summary>
    public bool SkipPlugins { get; set; } = false;

    /// <summary>
    /// The plugins to be skipped
    /// </summary>
    public PluginsToSkipOption[] PluginsToSkip { get; set; } = Enumerable.Empty<PluginsToSkipOption>().ToArray();
}

public class PluginsToSkipOption
{
    public string PluginName { get; set; } = string.Empty;

    public PluginFormat PluginFormat { get; set; }
}