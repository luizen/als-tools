namespace AlsTools.Core.ValueObjects
{
    public enum TrackType
    {
        /// <summary>
        /// Audio track
        /// </summary>
        Audio,

        /// <summary>
        /// MIDI track
        /// </summary>
        Midi,
        
        /// <summary>
        /// Return track
        /// </summary>
        Return,

        /// <summary>
        /// Group track (groups other tracks)
        /// </summary>
        Group,
        
        /// <summary>
        /// Master track
        /// </summary>
        Master

        //PreHear -> this is a track type I don't get yet what it means...
    }
}