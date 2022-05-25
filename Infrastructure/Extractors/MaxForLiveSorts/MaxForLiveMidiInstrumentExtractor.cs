using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.MaxForLiveSorts;

/// <summary>
/// MxDeviceInstrument
/// </summary>
public class MaxForLiveMidiInstrumentExtractor : BaseMaxForLiveSortExtractor
{
    private readonly ILogger<MaxForLiveMidiInstrumentExtractor> logger;

    public MaxForLiveMidiInstrumentExtractor(ILogger<MaxForLiveMidiInstrumentExtractor> logger) : base(logger, DeviceSort.MidiInstrument)
    {
        this.logger = logger;
    }
}


