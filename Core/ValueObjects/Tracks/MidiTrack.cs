namespace AlsTools.Core.ValueObjects.Tracks
{
    public class MidiTrack : BaseTrack, ITrack
    {
        public MidiTrack() : base(TrackType.Midi)
        {
        }
    }    
}