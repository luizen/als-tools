using AlsTools.Core.ValueObjects;

namespace AlsTools.Infrastructure.Extractors.Collections;

public interface ILocatorsCollectionExtractor : ICollectionExtractor<Locator>
{
}

public class LocatorsCollectionExtractor : ILocatorsCollectionExtractor
{
    private readonly ILogger<LocatorsCollectionExtractor> logger;

    public LocatorsCollectionExtractor(ILogger<LocatorsCollectionExtractor> logger)
    {
        this.logger = logger;
    }

    public IReadOnlyList<Locator> ExtractFromXml(XPathNavigator nav)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting Locators from XML...");

        var expression = $"/Ableton/LiveSet/Locators/Locators/Locator";
        var locatorsIterator = nav.Select(expression);
        var locators = new List<Locator>();

        foreach (XPathNavigator locatorNode in locatorsIterator)
        {
            var locator = new Locator()
            {
                Number = locatorNode.SelectSingleNode(@"@Id")?.ValueAsInt,
                Name = locatorNode.SelectSingleNode(@"Name/@Value")!.Value,
                Annotation = locatorNode.SelectSingleNode(@"Annotation/@Value")!.Value,
                Time = locatorNode.SelectSingleNode(@"Time/@Value")!.ValueAsInt,
                IsSongStart = locatorNode.SelectSingleNode(@"IsSongStart/@Value")!.ValueAsBoolean
            };

            locators.Add(locator);
        }

        return locators;
    }
}