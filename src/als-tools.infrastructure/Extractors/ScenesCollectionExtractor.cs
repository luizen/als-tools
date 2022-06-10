using AlsTools.Core.ValueObjects;

namespace AlsTools.Infrastructure.Extractors;

public interface IScenesCollectionExtractor : ICollectionExtractor<Scene>
{
}

public class ScenesCollectionExtractor : IScenesCollectionExtractor
{
    private readonly ILogger<ScenesCollectionExtractor> logger;

    public ScenesCollectionExtractor(ILogger<ScenesCollectionExtractor> logger)
    {
        this.logger = logger;
    }

    public IReadOnlyList<Scene> ExtractFromXml(XPathNavigator nav)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting Scenes from XML...");

        var expression = @"/Ableton/LiveSet/Scenes/Scene";
        var scenesIterator = nav.Select(expression);
        var scenes = new List<Scene>();

        foreach (XPathNavigator sceneNode in scenesIterator)
        {
            var scene = new Scene()
            {
                Number = sceneNode.SelectSingleNode(@"@Id")!.ValueAsInt,
                Name = sceneNode.SelectSingleNode(@"Name/@Value")!.Value,
                Annotation = sceneNode.SelectSingleNode(@"Annotation/@Value")!.Value,
                Tempo = sceneNode.SelectSingleNode(@"Tempo/@Value")!.ValueAsInt,
                IsTempoEnabled = sceneNode.SelectSingleNode(@"IsTempoEnabled/@Value")!.ValueAsBoolean,
                TimeSignatureId = sceneNode.SelectSingleNode(@"TimeSignatureId/@Value")!.ValueAsInt,
                IsTimeSignatureEnabled = sceneNode.SelectSingleNode(@"IsTimeSignatureEnabled/@Value")!.ValueAsBoolean
            };

            scenes.Add(scene);
        }

        return scenes;
    }
}