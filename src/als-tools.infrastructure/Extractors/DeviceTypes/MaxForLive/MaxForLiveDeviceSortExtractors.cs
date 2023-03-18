namespace AlsTools.Infrastructure.Extractors.DeviceTypes.MaxForLive;

public interface IMaxForLiveDeviceSortExtractor : IDeviceExtractor
{
}

/// <summary>
/// MxDeviceAudioEffect
/// </summary>
public class MaxForLiveAudioEffectDeviceSortExtractor : BaseMaxForLiveDeviceSortExtractor
{
    public MaxForLiveAudioEffectDeviceSortExtractor(ILogger<MaxForLiveAudioEffectDeviceSortExtractor> logger) : base(logger, DeviceSort.AudioEffect)
    {
    }
}


/// <summary>
/// MxDeviceMidiEffect
/// </summary>
public class MaxForLiveMidiEffectDeviceSortExtractor : BaseMaxForLiveDeviceSortExtractor
{
    public MaxForLiveMidiEffectDeviceSortExtractor(ILogger<MaxForLiveMidiEffectDeviceSortExtractor> logger) : base(logger, DeviceSort.MidiEffect)
    {
    }
}


/// <summary>
/// MxDeviceInstrument
/// </summary>
public class MaxForLiveMidiInstrumentDeviceSortExtractor : BaseMaxForLiveDeviceSortExtractor
{
    public MaxForLiveMidiInstrumentDeviceSortExtractor(ILogger<MaxForLiveMidiInstrumentDeviceSortExtractor> logger) : base(logger, DeviceSort.MidiInstrument)
    {
    }
}






