namespace AlsTools.Core.ValueObjects.Devices;

/// <summary>
/// Whether the device is an Instrument, Audio effect, Midi Effect or Midi Generic (could be Instrument or effect)
/// </summary>
/// <remarks>
/// From the Working with Instruments and Effects documentation:
/// "Every track in Live can host a number of devices. These devices can be of three different sorts:
/// - MIDI effects act upon MIDI signals and can only be placed in MIDI tracks.
/// - Audio effects act upon audio signals and can be placed in audio tracks. 
///     They can also be placed in MIDI tracks as long as they are “downstream“ from an instrument.
/// - Instruments are devices that reside in MIDI tracks, receive MIDI and output audio.
/// </remarks>
/// <seealso href="https://www.ableton.com/en/manual/working-with-instruments-and-effects/">Working with Instruments and Effects</seealso>
public enum DeviceSort
{
    MidiInstrument,

    AudioEffect,

    MidiEffect,

    /// <summary>
    /// In Vst2 and Vst3, it is impossible to differentiate Midi Instrument and Midi Effect.
    /// </summary>
    MidiInstrumentOrEffect,

    /// <summary>
    /// AU plugins are difficult (or even impossible) to get the sort from
    /// </summary>
    Unknown
}



// VST2 plugins
//     MIDI effect     -> Category = 2
//     MIDI instrument -> Category = 2 (no difference)
//     Audio FX        -> Category = 1

// VST3 plugins
//     MIDI effect     -> DeviceType = 1
//     MIDI instrument -> DeviceType = 1 (no difference)
//     Audio FX        -> DeviceType = 2

// AU plugins
//     MIDI effect     -> ?
//     MIDI instrument -> ?
//     Audio FX        -> ?

// Stock
//     MIDI effect     ->  <RelativePath Value="Devices/MIDI Effects/Arpeggiator  -->>   MidiArpeggiator/SourceContext/Value/BranchSourceContext/OriginalFileRef/FileRef/RelativePath/@Value
//     MIDI instrument ->  <RelativePath Value="Devices/Instruments/Wavetable
//     Audio FX        ->  <RelativePath Value="Devices/Audio Effects/EQ Eight    -->>   Eq8/SourceContext/Value/BranchSourceContext/OriginalFileRef/FileRef/RelativePath/@Value 

// Max4Live
//     MIDI effect     -> Devices/MxDeviceMidiEffect
//     MIDI instrument -> Devices/MxDeviceInstrument
//     Audio FX        -> Devices/MxDeviceAudioEffect

