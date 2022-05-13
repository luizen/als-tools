using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Tracks;

namespace AlsTools.Core.Factories
{
    public static class TrackFactory
    {
        public static ITrack CreateTrack(TrackType type, string effectiveName, string userName, string annotation, bool? isFrozen, TrackDelay trackDelay, int? parentGroupId = null)
        {
            ITrack track = type switch
            {
                TrackType.Audio => track = new AudioTrack(),
                TrackType.Midi => track = new MidiTrack(),
                TrackType.Return => track = new ReturnTrack(),
                TrackType.Group => track = new GroupTrack(),
                _ => track = new MasterTrack()
            };

            return SetDefaultProperties(track, effectiveName, userName, annotation, isFrozen, trackDelay, parentGroupId);
        }

        private static ITrack SetDefaultProperties(ITrack track, string effectiveName, string userName, string annotation, bool? isFrozen, TrackDelay trackDelay, int? parentGroupId = null)
        {
            track.EffectiveName = effectiveName;
            track.UserName = userName;
            track.Annotation = annotation;
            track.IsFrozen = isFrozen;
            track.TrackDelay = trackDelay;
            track.TrackGroupId = parentGroupId;
        
            return track;
        }
    }
}
