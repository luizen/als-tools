using AlsTools.Core.Entities;

namespace AlsTools.Infrastructure.Extractors.Collections;

public interface ILiveProjectsCollectionExtractor : ICollectionExtractor<LiveProject>
{
}

public class LiveProjectsCollectionExtractor : ILiveProjectsCollectionExtractor
{
    private readonly ILogger<LiveProjectsCollectionExtractor> logger;

    public LiveProjectsCollectionExtractor(ILogger<LiveProjectsCollectionExtractor> logger)
    {
        this.logger = logger;
    }

    public IReadOnlyList<LiveProject> ExtractFromXml(XPathNavigator nav)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting Live Project from XML...");

        var project = new LiveProject()
        {
            Creator = GetProjectAttribute<string>(nav, "Creator")!,
            MajorVersion = GetProjectAttribute<string>(nav, "MajorVersion")!,
            MinorVersion = GetProjectAttribute<string>(nav, "MinorVersion")!,
            SchemaChangeCount = GetProjectAttribute<int>(nav, "SchemaChangeCount"),
            Tempo = GetMasterTrackMixerAttribute<double>(nav, "Tempo"),
            TimeSignature = GetMasterTrackMixerAttribute<int>(nav, "TimeSignature"),
            GlobalGrooveAmount = GetMasterTrackMixerAttribute<int>(nav, "GlobalGrooveAmount")
        };

        return new List<LiveProject>() { project };
    }

    private T GetMasterTrackMixerAttribute<T>(XPathNavigator nav, string attribute)
    {
        var expression = $"/Ableton/LiveSet/MasterTrack/DeviceChain/Mixer/{attribute}/Manual/@Value";
        return GetXpathValue<T>(nav, expression);
    }

    private T GetProjectAttribute<T>(XPathNavigator nav, string attribute)
    {
        var expression = $"/Ableton/@{attribute}";
        return GetXpathValue<T>(nav, expression);
    }

    //TODO: extract this method to some util class
    private T GetXpathValue<T>(XPathNavigator nav, string expression)
    {
        var node = nav.SelectSingleNode(expression);
        if (node == null)
            return default(T)!;

        var result = (T)node.ValueAs(typeof(T));
        return result;
    }
}