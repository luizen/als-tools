using AlsTools.Core.Entities;

namespace AlsTools.Infrastructure.Extractors.Collections;

/// <summary>
/// Interface defining a collection extractor specific for Live Projects
/// </summary>
public interface ILiveProjectsCollectionExtractor : ICollectionExtractor<LiveProject>
{
}

/// <summary>
/// Collection extractor specific for Live Projects
/// </summary>
public class LiveProjectsCollectionExtractor : ILiveProjectsCollectionExtractor
{
    private readonly ILogger<LiveProjectsCollectionExtractor> logger;
    private readonly XpathExtractorHelper xpathExtractorHelper;

    public LiveProjectsCollectionExtractor(ILogger<LiveProjectsCollectionExtractor> logger, XpathExtractorHelper xpathExtractorHelper)
    {
        this.logger = logger;
        this.xpathExtractorHelper = xpathExtractorHelper;
    }

    public IReadOnlyList<LiveProject> ExtractFromXml(XPathNavigator nav)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting Live Project from XML...");

        var project = new LiveProject();

        TryGetProjectAttribute<string>(nav, "Creator", (result) => project.Creator = result);
        TryGetProjectAttribute<string>(nav, "MajorVersion", (result) => project.MajorVersion = result);
        TryGetProjectAttribute<string>(nav, "MinorVersion", (result) => project.MinorVersion = result);
        TryGetProjectAttribute<int>(nav, "SchemaChangeCount", (result) => project.SchemaChangeCount = result);
        TryGetMasterTrackMixerAttribute<double>(nav, "Tempo", (result) => project.Tempo = result);
        TryGetMasterTrackMixerAttribute<int>(nav, "TimeSignature", (result) => project.TimeSignature = result);
        TryGetMasterTrackMixerAttribute<double>(nav, "GlobalGrooveAmount", (result) => project.GlobalGrooveAmount = result);

        return new List<LiveProject>() { project };
    }

    private void TryGetMasterTrackMixerAttribute<T>(XPathNavigator nav, string attribute, Action<T> successAction)
    {
        string[] expressionsToTry =
        {
            $"/Ableton/LiveSet/MasterTrack/DeviceChain/Mixer/{attribute}/Manual/@Value",
            $"/Ableton/LiveSet/MasterTrack/MasterChain/Mixer/{attribute}/ArrangerAutomation/Events/*/@Value"
        };

        xpathExtractorHelper.TryGetOneOfXpathValues(nav, expressionsToTry, successAction);
    }

    private void TryGetProjectAttribute<T>(XPathNavigator nav, string attribute, Action<T> successAction)
    {
        var expression = $"/Ableton/@{attribute}";
        xpathExtractorHelper.TryGetXpathValue(nav, expression, successAction);
    }
}