using System;
using AlsTools.Core.ValueObjects;

namespace AlsTools.Infrastructure.Extractors.Collections;

/// <summary>
/// Interface defining a collection extractor specific for Samples
/// </summary>
public interface ISamplesCollectionExtractor : ICollectionExtractor<SampleRef>
{
}

public class SamplesCollectionExtractor : ISamplesCollectionExtractor
{
    private readonly ILogger<SamplesCollectionExtractor> logger;

    public SamplesCollectionExtractor(ILogger<SamplesCollectionExtractor> logger)
    {
        this.logger = logger;
    }

    public IReadOnlyList<SampleRef> ExtractFromXml(XPathNavigator nav)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting Samples from XML...");
        
        var expression = $".//SampleRef/FileRef/Path/@Value";
        var sampleRefsIterator = nav.Select(expression);
        var sampleRefs = new List<SampleRef>();

        foreach (XPathNavigator sampleRefNode in sampleRefsIterator)
        {
            var sampleRef = new SampleRef()
            {
                FileRefPath = sampleRefNode.Value
            };
            
            sampleRefs.Add(sampleRef);
        }

        return sampleRefs;
    }
}
