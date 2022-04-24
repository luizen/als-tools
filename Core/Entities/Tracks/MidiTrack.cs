namespace AlsTools.Core.Entities.Tracks
{
    public class MidiTrack : BaseTrack, ITrack
    {
        public MidiTrack() : base(TrackType.Midi)
        {
        }
    }    
}