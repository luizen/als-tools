using AlsTools.Core.Factories;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Tracks;

namespace AlsTools.Infrastructure.Handlers;

public class TrackExtractionHandler : ITrackExtractionHandler
{
    private readonly ILogger<TrackExtractionHandler> logger;

    private readonly IDeviceExtractionHandler deviceExtractionHandler;

    public TrackExtractionHandler(ILogger<TrackExtractionHandler> logger, IDeviceExtractionHandler deviceExtractionHandler)
    {
        this.logger = logger;
        this.deviceExtractionHandler = deviceExtractionHandler;
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
            var effectiveName = trackNode.SelectSingleNode(@"Name/EffectiveName/@Value")!.Value;
            var userName = trackNode.SelectSingleNode(@"Name/UserName/@Value")!.Value;
            var annotation = trackNode.SelectSingleNode(@"Name/Annotation/@Value")!.Value;
            var isFrozen = trackNode.SelectSingleNode(@"Freeze/@Value")?.ValueAsBoolean;
            var groupId = trackNode.SelectSingleNode(@"TrackGroupId/@Value")!.ValueAsInt;
            var trackDelay = new TrackDelay()
            {
                Value = trackNode.SelectSingleNode(@"TrackDelay/Value/@Value")?.ValueAsInt,
                IsValueSampleBased = trackNode.SelectSingleNode(@"TrackDelay/IsValueSampleBased/@Value")?.ValueAsBoolean
            };

            // Create the track
            var track = TrackFactory.CreateTrack(trackType, id, effectiveName, userName, annotation, isFrozen, trackDelay, groupId);

            logger.LogDebug(@"Extracted Track name: {@TrackName}", track.EffectiveName);

            // Now let's get all devices in this track
            var devices = deviceExtractionHandler.ExtractFromXml(trackNode);

            track.AddDevices(devices);

            tracks.Add(track);
        }
    }
}