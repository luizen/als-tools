using Humanizer;

namespace AlsTools.Infrastructure.XmlNodeNames;

public static class LiveStockDeviceNodeNames
{
    /// <summary>
    /// A dictionary associating a stock device XML Node internal name to its readable name, which sometimes is different 
    /// </summary>
    private static readonly IReadOnlyDictionary<string, string> stockDeviceNamesByNodeInternalName;

    static LiveStockDeviceNodeNames()
    {
        var dic = new Dictionary<string, string>();

        var nestedClassTypes = typeof(LiveStockDeviceNodeNames).GetNestedTypes(BindingFlags.Static | BindingFlags.Public);

        foreach (var classType in nestedClassTypes)
        {
            var fields = classType.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields)
            {
                var key = field.GetValue(null).ToString().ToUpperInvariant();
                var value = field.Name.Humanize(LetterCasing.Title);

                dic.Add(key, value);
            }
        }

        stockDeviceNamesByNodeInternalName = new Dictionary<string, string>(dic);
    }

    public static string GetDeviceNameByNodeName(string nodeName)
    {
        var key = nodeName.ToUpperInvariant();
        string value = null;

        if (stockDeviceNamesByNodeInternalName.TryGetValue(key, out value))
            return value;

        return nodeName;
    }

    public static class MidiEffects
    {
        public const string Arpegiator = "MidiArpeggiator";
        public const string Chord = "MidiChord";
        // public const string EnvelopeMidi = "EnvelopeMidi";
        // public const string ExpressionControl = "ExpressionControl";
        // public const string Microtuner = "Microtuner";
        public const string MidiEffectRack = "MidiEffectGroupDevice";
        // public const string MidiMonitor = "MidiMonitor";
        // public const string MpeControl = "MpeControl";
        // public const string NoteEcho = "NoteEcho";
        public const string NoteLength = "MidiNoteLength";
        public const string Pitch = "MidiPitcher";
        public const string Random = "MidiRandom";
        // public const string RotatingRhythmGenerator = "RotatingRhythmGenerator";
        public const string Scale = "MidiScale";
        // public const string ShaperMidi = "ShaperMidi";
        public const string Velocity = "MidiVelocity";

    }

    public static class MidiInstruments
    {
        public const string Analog = "UltraAnalog";
        public const string Collision = "Collision";
        // public const string CvInstrument = "CvInstrument";
        // public const string CvTriggers = "CvTriggers";
        public const string DrumRack = "DrumGroupDevice";
        public const string Electric = "LoungeLizard";
        public const string ExternalInstrument = "ProxyInstrumentDevice";
        public const string Impulse = "InstrumentImpulse";
        public const string InstrumentRack = "InstrumentGroupDevice";
        public const string Operator = "Operator";
        public const string Sampler = "MultiSampler";
        public const string Simpler = "OriginalSimpler";
        public const string Tension = "StringStudio";
        public const string Wavetable = "InstrumentVector";

        // public const string DsClang = "DsClang";
        // public const string DsClap = "DsClap";
        // public const string DsCymbal = "DsCymbal";
        // public const string DsFm = "DsFm";
        // public const string DsHh = "DsHh";
        // public const string DsKick = "DsKick";
        // public const string DsSampler = "DsSampler";
        // public const string DsSnare = "DsSnare";
        // public const string DsTom = "DsTom";
    }

    public static class AudioEffects
    {
        // public const string AlignDelay = "AlignDelay";

        public const string Amp = "Amp";
        public const string AudioEffectRack = "AudioEffectGroupDevice";
        public const string AutoFilter = "AutoFilter";
        public const string AutoPan = "AutoPan";
        public const string BeatRepeat = "BeatRepeat";
        public const string Cabinet = "Cabinet";
        public const string ChannelEq = "ChannelEq";
        public const string ChorusEnsemble = "Chorus2";
        public const string Compressor = "Compressor2";
        public const string Corpus = "Corpus";

        // public const string CvClockIn = "CvClockIn";
        // public const string CvClockOut = "CvClockOut";
        // public const string CvEnvelopeFollower = "CvEnvelopeFollower";
        // public const string CvIn = "CvIn";
        // public const string CvLfo = "CvLfo";
        // public const string CvShaper = "CvShaper";
        // public const string CvUtility = "CvUtility";
        public const string Delay = "Delay";
        public const string DrumBuss = "DrumBuss";
        public const string DynamicTube = "Tube";
        public const string Echo = "Echo";
        // public const string EnvelopeFollower = "EnvelopeFollower";
        public const string Eq8 = "Eq8";
        public const string Eq3 = "FilterEQ3";
        public const string Erosion = "Erosion";
        public const string ExternalAudioEffect = "ProxyAudioEffectDevice";
        public const string FilterDelay = "FilterDelay";
        public const string Gate = "Gate";
        public const string GlueCompressor = "GlueCompressor";
        public const string GrainDelay = "GrainDelay";
        public const string HybridReverb = "Hybrid";

        // public const string Lfo = "Lfo";

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
        // public const string Shaper = "Shaper";
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