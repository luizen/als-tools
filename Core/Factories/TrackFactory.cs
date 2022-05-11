using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Tracks;

namespace AlsTools.Core.Factories
{
    public static class TrackFactory
    {
        public static ITrack CreateTrack(TrackType type, string name)
        {
            switch (type)
            {
                case TrackType.Audio:
                    return new AudioTrack() { EffectiveName = name };

                case TrackType.Midi:
                    return new MidiTrack() { EffectiveName = name };

                case TrackType.Return:
                    return new ReturnTrack() { EffectiveName = name };

                case TrackType.Group:
                    return new GroupTrack() { EffectiveName = name };

                default:
                    return new MasterTrack() { EffectiveName = name };
            }
        }
    }
}
