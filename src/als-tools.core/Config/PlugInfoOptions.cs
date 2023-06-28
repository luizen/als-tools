namespace AlsTools.Core.Config;

/// <summary>
/// PlugInfo options, to be configured via appsettings.json file
/// </summary>
public class PlugInfoOptions
{
    /// <summary>
    /// The file to use as input
    /// </summary>
    public string InputFilePath { get; set; } = string.Empty;

    /// <summary>
    /// The plugin paths to be used
    /// </summary>
    public PluginPathOptions PluginPathOptions { get; set; } = new PluginPathOptions();
}

public class PluginPathOptions
{
    public string[] Vst2Paths { get; set; } = {};

    public string[] Vst3Paths { get; set; } = {};
    
    public string[] AudioUnitPaths { get; set; } = {};
    
    public string[] ClapPaths { get; set; } = {};
}