namespace AlsTools.Infrastructure.Extractors.Collections;

/// <summary>
/// Interface defining a collection extractor specific for Samples
/// </summary>
public interface ISamplesCollectionExtractor : ICollectionExtractor<string>
{
}

public class SamplesCollectionExtractor : ISamplesCollectionExtractor
{
    private readonly ILogger<SamplesCollectionExtractor> logger;

    public SamplesCollectionExtractor(ILogger<SamplesCollectionExtractor> logger)
    {
        this.logger = logger;
    }

    public IReadOnlyList<string> ExtractFromXml(XPathNavigator nav)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting Samples from XML...");

        var expression = $".//SampleRef/FileRef/Path/@Value";
        var sampleRefsIterator = nav.Select(expression);
        var sampleRefs = new List<string>();

        foreach (XPathNavigator sampleRefNode in sampleRefsIterator)
        {
            sampleRefs.Add(sampleRefNode.Value);
        }

        return sampleRefs;
    }
}
