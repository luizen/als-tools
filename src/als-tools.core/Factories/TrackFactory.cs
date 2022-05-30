using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Tracks;

namespace AlsTools.Core.Factories;

public static class TrackFactory
{
    public static ITrack CreateTrack(TrackType type, int id, string effectiveName, string userName, string annotation, bool? isFrozen, TrackDelay trackDelay, int parentGroupId)
    {
        ITrack track = type switch
        {
            TrackType.Audio => new AudioTrack(),
            TrackType.Midi => new MidiTrack(),
            TrackType.Return => new ReturnTrack(),
            TrackType.Group => new GroupTrack(),
            _ => new MasterTrack()
        };

        return SetDefaultProperties(track, id, effectiveName, userName, annotation, isFrozen, trackDelay, parentGroupId);
    }

    private static ITrack SetDefaultProperties(ITrack track, int id, string effectiveName, string userName, string annotation, bool? isFrozen, TrackDelay trackDelay, int parentGroupId)
    {
        track.Id = id;
        track.EffectiveName = effectiveName;
        track.UserName = userName;
        track.Annotation = annotation;
        track.IsFrozen = isFrozen;
        track.TrackDelay = trackDelay;
        track.TrackGroupId = parentGroupId;

        return track;
    }
}
