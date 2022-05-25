namespace AlsTools.Core.ValueObjects;

public enum TrackType
{
    /// <summary>
    /// Audio track
    /// </summary>
    Audio = 1,

    /// <summary>
    /// MIDI track
    /// </summary>
    Midi = 2,

    /// <summary>
    /// Group track (groups other tracks)
    /// </summary>
    Group = 3,

    /// <summary>
    /// Return track
    /// </summary>
    Return = 4,

    /// <summary>
    /// Master track
    /// </summary>
    Master = 5

    //PreHear -> this is a track type I don't get yet what it means...
}