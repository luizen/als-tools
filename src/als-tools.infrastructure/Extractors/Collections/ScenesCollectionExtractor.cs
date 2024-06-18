using AlsTools.Core.ValueObjects;

namespace AlsTools.Infrastructure.Extractors.Collections;

/// <summary>
/// Interface defining a collection extractor specific for Scenes
/// </summary>
public interface IScenesCollectionExtractor : ICollectionExtractor<Scene>
{
}

/// <summary>
/// Collection extractor specific for Scenes
/// </summary>
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
                IsTimeSignatureEnabled = sceneNode.SelectSingleNode(@"IsTimeSignatureEnabled/@Value")!.ValueAsBoolean,
                Color = LiveColor.FromValue(sceneNode.SelectSingleNode(@"Color/@Value")?.ValueAsInt)
            };

            scenes.Add(scene);
        }

        return scenes;
    }
}