using AlsTools.Infrastructure.Attributes;
using AlsTools.Infrastructure.Extractors.StockDevices.StockAudioEffects;
using AlsTools.Infrastructure.Extractors.StockDevices.StockInstruments;
using AlsTools.Infrastructure.Extractors.StockDevices.StockMidiEffects;
using AlsTools.Infrastructure.Extractors.StockDevices.StockRacks;

namespace AlsTools.Infrastructure.XmlNodeNames;

public static class LiveStockDeviceNodeNames
{
    /// <summary>
    /// A dictionary associating a stock device XML Node internal name to its readable name, which sometimes is different
    /// </summary>
    private static readonly IReadOnlyDictionary<string, string> stockDeviceNamesByNodeInternalName;

    static LiveStockDeviceNodeNames()
    {
        stockDeviceNamesByNodeInternalName = BuildDeviceNamesByNodeInternalNames();
    }

    private static Dictionary<string, string> BuildDeviceNamesByNodeInternalNames()
    {
        var dic = new Dictionary<string, string>();

        var nestedClassTypes = typeof(LiveStockDeviceNodeNames).GetNestedTypes(BindingFlags.Static | BindingFlags.Public);

        foreach (var classType in nestedClassTypes)
        {
            var fields = classType.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields)
            {
                var key = field.GetValue(null)!.ToString()!.ToUpperInvariant();
                var value = field.Name.Humanize(LetterCasing.Title);

                dic.Add(key, value);
            }
        }

        return new Dictionary<string, string>(dic);
    }

    public static string GetDeviceNameByNodeName(string nodeName)
    {
        var key = nodeName.ToUpperInvariant();
        string? value;

        if (stockDeviceNamesByNodeInternalName.TryGetValue(key, out value))
            return value;

        return nodeName;
    }

    // //QUESTION: put into a separate class or leave them where there were, in their original groups?
    // public static class Racks
    // {
    //     [StockDeviceExtractor(typeof(AudioEffectRackExtractor))]
    //     public const string AudioEffectRack = "AudioEffectGroupDevice";

    //     [StockDeviceExtractor(typeof(MidiInstrumentRackExtractor))]
    //     public const string InstrumentRack = "InstrumentGroupDevice";

    //     [StockDeviceExtractor(typeof(DrumRackExtractor))]
    //     public const string DrumRack = "DrumGroupDevice";

    //     [StockDeviceExtractor(typeof(MidiEffectRackExtractor))]
    //     public const string MidiEffectRack = "MidiEffectGroupDevice";
    // }

    [ExtractingStockDevice(typeof(ICommonStockMidiEffectExtractor))]
    public static class MidiEffects
    {
        [ExtractingStockDevice(typeof(MidiEffectRackExtractor))]
        public const string MidiEffectRack = "MidiEffectGroupDevice";

        public const string Arpegiator = "MidiArpeggiator";

        public const string Chord = "MidiChord";

        public const string NoteLength = "MidiNoteLength";

        public const string Pitch = "MidiPitcher";

        public const string Random = "MidiRandom";

        public const string Scale = "MidiScale";

        public const string Velocity = "MidiVelocity";
    }

    [ExtractingStockDevice(typeof(ICommonStockInstrumentExtractor))]
    public static class MidiInstruments
    {
        [ExtractingStockDevice(typeof(DrumRackExtractor))]
        public const string DrumRack = "DrumGroupDevice";

        [ExtractingStockDevice(typeof(MidiInstrumentRackExtractor))]
        public const string InstrumentRack = "InstrumentGroupDevice";

        public const string Analog = "UltraAnalog";

        public const string Collision = "Collision";

        public const string Electric = "LoungeLizard";

        public const string ExternalInstrument = "ProxyInstrumentDevice";

        public const string Impulse = "InstrumentImpulse";

        public const string Operator = "Operator";

        public const string Sampler = "MultiSampler";

        public const string Simpler = "OriginalSimpler";

        public const string Tension = "StringStudio";

        public const string Wavetable = "InstrumentVector";
    }

    [ExtractingStockDevice(typeof(ICommonStockAudioEffectExtractor))]
    public static class AudioEffects
    {
        [ExtractingStockDevice(typeof(AudioEffectRackExtractor))]
        public const string AudioEffectRack = "AudioEffectGroupDevice";

        public const string Amp = "Amp";

        public const string AutoFilter = "AutoFilter";

        public const string AutoPan = "AutoPan";

        public const string BeatRepeat = "BeatRepeat";

        public const string Cabinet = "Cabinet";

        public const string ChannelEq = "ChannelEq";

        public const string ChorusEnsemble = "Chorus2";

        public const string Compressor = "Compressor2";

        public const string Corpus = "Corpus";

        public const string Delay = "Delay";

        public const string DrumBuss = "DrumBuss";

        public const string DynamicTube = "Tube";

        public const string Echo = "Echo";

        public const string Eq8 = "Eq8";

        public const string Eq3 = "FilterEQ3";

        public const string Erosion = "Erosion";

        public const string ExternalAudioEffect = "ProxyAudioEffectDevice";

        public const string FilterDelay = "FilterDelay";

        public const string Gate = "Gate";

        public const string GlueCompressor = "GlueCompressor";

        public const string GrainDelay = "GrainDelay";

        public const string HybridReverb = "Hybrid";

        public const string Limiter = "Limiter";

        public const string Looper = "Looper";

        public const string MultibandDynamics = "MultibandDynamics";

        public const string Overdrive = "Overdrive";

        public const string Pedal = "Pedal";

        public const string PhaserFlanger = "PhaserNew";

        public const string Redux = "Redux2";

        public const string Resonators = "Resonator";

        public const string Reverb = "Reverb";

        public const string Saturator = "Saturator";

        public const string Shifter = "Shifter";

        public const string SpectralResonator = "Transmute";

        public const string SpectralTime = "Spectral";

        public const string Spectrum = "SpectrumAnalyzer";

        public const string Tuner = "Tuner";

        public const string Utility = "StereoGain";

        public const string VinylDistortion = "Vinyl";

        public const string Vocoder = "Vocoder";
    }
}