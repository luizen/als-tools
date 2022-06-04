using AlsTools.Infrastructure.Extractors.StockDevices;

namespace AlsTools.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
public sealed class ExtractingStockDeviceAttribute : Attribute
{
    public Type DeviceExtractorType { get; }

    public ExtractingStockDeviceAttribute(Type deviceExtractorType)
    {
        if (!typeof(IStockDeviceExtractor).IsAssignableFrom(deviceExtractorType))
            throw new ArgumentException($"The especified device extractor type must implement the interface {nameof(IStockDeviceExtractor)}.");

        DeviceExtractorType = deviceExtractorType;
    }
}