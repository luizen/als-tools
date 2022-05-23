using AlsTools.Core.ValueObjects.Devices;
using Microsoft.Extensions.Logging;

namespace AlsTools.Infrastructure.Extractors.MaxForLiveSorts;

/// <summary>
/// MxDeviceMidiEffect
/// </summary>
public class MaxForLiveMidiEffectExtractor : BaseMaxForLiveSortExtractor
{
    private readonly ILogger<MaxForLiveMidiEffectExtractor> logger;

    public MaxForLiveMidiEffectExtractor(ILogger<MaxForLiveMidiEffectExtractor> logger) : base(logger, DeviceSort.MidiEffect)
    {
        this.logger = logger;
    }
}


