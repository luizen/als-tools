using AlsTools.Core.Entities;
using AlsTools.Core.Entities.Tracks;

namespace AlsTools.Core.Factories
{
    public static class TrackFactory
    {
        public static ITrack CreateTrack(TrackType type, string name)
        {
            switch (type)
            {
                case TrackType.Audio:
                    return new AudioTrack() { Name = name };
                
                case TrackType.Midi:
                    return new MidiTrack() { Name = name };

                case TrackType.Return:
                    return new ReturnTrack() { Name = name };
                
                default:
                    return new MasterTrack() { Name = name };
            }
        }
    }
}
