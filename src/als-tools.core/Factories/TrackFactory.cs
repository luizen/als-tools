using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Tracks;

namespace AlsTools.Core.Factories;

public static class TrackFactory
{
    public static ITrack CreateTrack(TrackType type, int? id, string? effectiveName, string userName, string annotation, bool? isFrozen, bool? isMuted, bool? isSoloed, TrackDelay trackDelay, int parentGroupId, int? color)
    {
        ITrack track = type switch
        {
            TrackType.Audio => new AudioTrack(),
            TrackType.Midi => new MidiTrack(),
            TrackType.Return => new ReturnTrack(),
            TrackType.Group => new GroupTrack(),
            _ => new MasterTrack()
        };

        return SetDefaultProperties(track, id, effectiveName, userName, annotation, isFrozen, isMuted, isSoloed, trackDelay, parentGroupId, color);
    }

    private static ITrack SetDefaultProperties(ITrack track, int? id, string? effectiveName, string userName, string annotation, bool? isFrozen, bool? isMuted, bool? isSoloed, TrackDelay trackDelay, int parentGroupId, int? color)
    {
        track.Id = id;
        track.EffectiveName = effectiveName;
        track.UserName = userName;
        track.Annotation = annotation;
        track.IsFrozen = isFrozen;
        track.IsMuted = isMuted;
        track.IsSoloed = isSoloed;
        track.TrackDelay = trackDelay;
        track.TrackGroupId = parentGroupId;
        track.Color = LiveColor.FromValue(color);
        return track;
    }
}
