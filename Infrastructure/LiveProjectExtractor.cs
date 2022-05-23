using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.XPath;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using AlsTools.Infrastructure.Handlers;
using Microsoft.Extensions.Logging;

namespace AlsTools.Infrastructure;

public class LiveProjectExtractor : ILiveProjectExtractor
{
    ILogger<LiveProjectExtractor> logger;
    private readonly ILiveProjectExtractionHandler liveProjectExtractionHandler;
    private readonly ITrackExtractionHandler trackExtractionHandler;
    private readonly ISceneExtractionHandler sceneExtractionHandler;
    private readonly ILocatorExtractionHandler locatorExtractionHandler;

    public LiveProjectExtractor(ILogger<LiveProjectExtractor> logger,
        ILiveProjectExtractionHandler liveProjectExtractionHandler,
        ITrackExtractionHandler trackExtractionHandler,
        ISceneExtractionHandler sceneExtractionHandler,
        ILocatorExtractionHandler locatorExtractionHandler)
    {
        this.logger = logger;
        this.liveProjectExtractionHandler = liveProjectExtractionHandler;
        this.trackExtractionHandler = trackExtractionHandler;
        this.sceneExtractionHandler = sceneExtractionHandler;
        this.locatorExtractionHandler = locatorExtractionHandler;
    }

    public LiveProject ExtractProjectFromFile(FileInfo file)
    {
        logger.LogDebug("Extracting project from file {file}", file.FullName);

        using (FileStream originalFileStream = file.OpenRead())
        {
            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
            {
                using (StreamReader unzip = new StreamReader(decompressionStream))
                {
                    var xPathDoc = new XPathDocument(unzip);
                    var nav = xPathDoc.CreateNavigator();

                    var project = ExtractProject(file.Name, file.FullName, nav);
                    return project;
                }
            }
        }
    }

    private LiveProject ExtractProject(string fileName, string fullPath, XPathNavigator nav)
    {
        var project = liveProjectExtractionHandler.ExtractFromXml(nav).Single();
        project.Name = fileName;
        project.Path = fullPath;
        project.Scenes = sceneExtractionHandler.ExtractFromXml(nav);
        project.Locators = locatorExtractionHandler.ExtractFromXml(nav);
        project.Tracks = trackExtractionHandler.ExtractFromXml(nav);

        return project;
    }
}
