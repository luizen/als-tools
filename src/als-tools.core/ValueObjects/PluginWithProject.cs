using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.ValueObjects;

public class PluginWithProject
{
    public PluginWithProject(string projectPath, string pluginName, PluginFormat pluginFormat)
    {
        ProjectPath = projectPath;
        PluginName = pluginName;
        PluginFormat = pluginFormat;
    }

    public string ProjectPath { get; }

    public string PluginName { get; }

    public PluginFormat PluginFormat { get; }

    public string PluginFormatAsString
    {
        get
        {
            return PluginFormat.ToString();
        }
    }
}