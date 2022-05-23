using AlsTools.Core.ValueObjects.Devices;
using Microsoft.Extensions.Logging;

namespace AlsTools.Infrastructure.Extractors.MaxForLiveSorts;

/// <summary>
/// MxDeviceAudioEffect
/// </summary>
public class MaxForLiveAudioEffectExtractor : BaseMaxForLiveSortExtractor
{
    private readonly ILogger<MaxForLiveAudioEffectExtractor> logger;

    public MaxForLiveAudioEffectExtractor(ILogger<MaxForLiveAudioEffectExtractor> logger) : base(logger, DeviceSort.AudioEffect)
    {
        this.logger = logger;
    }
}


