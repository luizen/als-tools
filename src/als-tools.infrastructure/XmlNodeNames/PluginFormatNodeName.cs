using System.Collections.ObjectModel;
using AlsTools.Core.ValueObjects;

namespace AlsTools.Infrastructure.XmlNodeNames;

public static class PluginFormatNodeName
{
    public const string VST2 = "VSTPLUGININFO";

    public const string VST3 = "VST3PLUGININFO";

    public const string AU = "AUPLUGININFO";

    //TODO: implement AUV3. The XML node name is the same as of AUv2. 
    /* It seems the only way to differentiate them is by checking AuPluginDevice/SourceContext/Value/BranchSourceContext/BrowserContentPath/Value
    Example:
    <AuPluginDevice Id="0">
        <LockedScripts />
        <IsFolded Value="false" />
        <ShouldShowPresetName Value="true" />
        <UserName Value="" />
        <Annotation Value="" />
        <SourceContext>
            <Value>
                <BranchSourceContext Id="0">
                    <OriginalFileRef />
                    <BrowserContentPath Value="query:Plugins#AUv3:Moog:Model%2015" />
    
    */
    // public const string AUv3 = "AUPLUGININFO";  

    private static readonly IReadOnlyDictionary<PluginFormat, string> nodeNamesByPluginFormat = new ReadOnlyDictionary<PluginFormat, string>
    (
        new Dictionary<PluginFormat, string>()
        {
            [PluginFormat.AUv2] = AU,
            [PluginFormat.VST2] = VST2,
            [PluginFormat.VST3] = VST3
        }
    );

    private static readonly IReadOnlyDictionary<string, PluginFormat> pluginFormatsByNodeNames = new ReadOnlyDictionary<string, PluginFormat>
    (
        new Dictionary<string, PluginFormat>()
        {
            [AU] = PluginFormat.AUv2,
            [VST2] = PluginFormat.VST2,
            [VST3] = PluginFormat.VST3
        }
    );

    public static string GetNodeNameFromPluginFormat(PluginFormat format)
    {
        return nodeNamesByPluginFormat[format];
    }

    public static PluginFormat GetPluginFormatFromNodeName(string nodeName)
    {
        return pluginFormatsByNodeNames[nodeName];
    }
}