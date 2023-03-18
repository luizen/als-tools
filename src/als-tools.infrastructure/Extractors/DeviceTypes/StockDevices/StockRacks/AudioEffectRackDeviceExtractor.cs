namespace AlsTools.Infrastructure.Extractors.DeviceTypes.StockDevices.StockRacks;

public class AudioEffectRackDeviceExtractor : BaseRackDeviceExtractor, IStockRackDeviceExtractor
{
    public override string XPathDevicesSelector => @"./Branches/AudioEffectBranch/DeviceChain/AudioToAudioDeviceChain/Devices/*";

    public AudioEffectRackDeviceExtractor(ILogger<AudioEffectRackDeviceExtractor> logger, Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>> deviceTypeExtractors, IDictionary<string, DeviceType> deviceTypesByNodeDesc) : base(logger, deviceTypeExtractors, deviceTypesByNodeDesc)
    {
    }

    protected override IDevice CreateDevice()
    {
        return new AudioEffectRackDevice();
    }
}