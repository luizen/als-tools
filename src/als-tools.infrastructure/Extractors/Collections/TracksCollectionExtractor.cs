using AlsTools.Core.Factories;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Tracks;

namespace AlsTools.Infrastructure.Extractors.Collections;

/// <summary>
/// Interface defining a collection extractor specific for Tracks
/// </summary>
public interface ITracksCollectionExtractor : ICollectionExtractor<ITrack>
{
}


/// <summary>
/// Collection extractor specific for Tracks
/// </summary>
public class TracksCollectionExtractor : ITracksCollectionExtractor
{
    private readonly ILogger<TracksCollectionExtractor> logger;

    private readonly IDevicesCollectionExtractor devicesCollectionExtractor;
    private readonly ISamplesCollectionExtractor samplesCollectionExtractor;
    private readonly XpathExtractorHelper xpathExtractorHelper;

    public TracksCollectionExtractor(ILogger<TracksCollectionExtractor> logger, IDevicesCollectionExtractor devicesCollectionExtractor, ISamplesCollectionExtractor samplesCollectionExtractor, XpathExtractorHelper xpathExtractorHelper)
    {
        this.logger = logger;
        this.devicesCollectionExtractor = devicesCollectionExtractor;
        this.samplesCollectionExtractor = samplesCollectionExtractor;
        this.xpathExtractorHelper = xpathExtractorHelper;
    }

    public IReadOnlyList<ITrack> ExtractFromXml(XPathNavigator nav)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting Tracks from XML...");

        var tracks = new List<ITrack>();

        var expression = @"/Ableton/LiveSet/Tracks/AudioTrack";
        GetTrackByExpression(tracks, nav, expression, TrackType.Audio);

        expression = @"/Ableton/LiveSet/Tracks/MidiTrack";
        GetTrackByExpression(tracks, nav, expression, TrackType.Midi);

        expression = @"/Ableton/LiveSet/Tracks/ReturnTrack";
        GetTrackByExpression(tracks, nav, expression, TrackType.Return);

        expression = @"/Ableton/LiveSet/Tracks/GroupTrack";
        GetTrackByExpression(tracks, nav, expression, TrackType.Group);

        expression = @"/Ableton/LiveSet/MasterTrack";
        GetTrackByExpression(tracks, nav, expression, TrackType.Master);

        return tracks;
    }

    private void GetTrackByExpression(IList<ITrack> tracks, XPathNavigator nav, string expression, TrackType trackType)
    {
        logger.LogDebug("Extracting tracks of type: {@TrackType}", trackType);

        var tracksIterator = nav.Select(expression);

        // Iterate through the tracks of the same type (audio, midi, return, master)
        foreach (XPathNavigator trackNode in tracksIterator)
        {
            var id = trackNode.SelectSingleNode(@"@Id")?.ValueAsInt;
            var effectiveName = trackNode.SelectSingleNode(@"Name/EffectiveName/@Value")?.Value;
            var userName = trackNode.SelectSingleNode(@"Name/UserName/@Value")!.Value;
            var annotation = trackNode.SelectSingleNode(@"Name/Annotation/@Value")!.Value;
            var groupId = trackNode.SelectSingleNode(@"TrackGroupId/@Value")!.ValueAsInt;
            var isFrozen = trackNode.SelectSingleNode(@"Freeze/@Value")?.ValueAsBoolean;
            var color = trackNode.SelectSingleNode(@"Color/@Value")?.ValueAsInt;
            bool? isMuted = default;
            bool? isSoloed = default;

            //TODO: handle solo/cue in the Master / Cue track. Test Mute and Solo using an audio interface with more than 1 output (my Apogee)
            // More info: https://www.ableton.com/en/manual/mixing/#soloing-and-cueing
            ExtractIsMutedProperty(trackNode, (result) => isMuted = !result);
            ExtractIsSoloedProperty(trackNode, (result) => isSoloed = result);

            var trackDelay = new TrackDelay()
            {
                Value = trackNode.SelectSingleNode(@"TrackDelay/Value/@Value")?.ValueAsDouble,
                IsValueSampleBased = trackNode.SelectSingleNode(@"TrackDelay/IsValueSampleBased/@Value")?.ValueAsBoolean
            };

            // Create the track
            var track = TrackFactory.CreateTrack(trackType, id, effectiveName, userName, annotation, isFrozen, isMuted, isSoloed, trackDelay, groupId, color);

            logger.LogDebug(@"Extracted Track name: {@TrackName}", track.EffectiveName);

            // Now let's get all devices in this track
            var devices = devicesCollectionExtractor.ExtractFromXml(trackNode);
            track.AddDevices(devices);

            // Now let's get all samples in this track
            var samples = samplesCollectionExtractor.ExtractFromXml(trackNode);
            track.AddSamples(samples);

            tracks.Add(track);
        }
    }

    private bool ExtractIsMutedProperty(XPathNavigator nav, Action<bool> successAction)
    {
        string[] isMutedExpressions =
            {
                "DeviceChain/Mixer/Speaker/Manual/@Value",
                "DeviceChain/Mixer/Speaker/ArrangerAutomation/Events/*/@Value",    // for older versions
                "MasterChain/Mixer/Speaker/ArrangerAutomation/Events/*/@Value"     // for older versions
            };

        return xpathExtractorHelper.TryGetOneOfXpathValues(nav, isMutedExpressions, successAction);
    }

    private bool ExtractIsSoloedProperty(XPathNavigator nav, Action<bool> successAction)
    {
        string[] isSoloedExpressions =
            {
                "DeviceChain/Mixer/SoloSink/@Value",
                "MasterChain/Mixer/SoloSink/@Value"    // for older versions
            };

        return xpathExtractorHelper.TryGetOneOfXpathValues(nav, isSoloedExpressions, successAction);
    }
}