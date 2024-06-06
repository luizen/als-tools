using AlsTools.Core.Entities;
using AlsTools.Core.Enums;
using AlsTools.Core.Interfaces;
using AlsTools.Infrastructure.Extractors.Collections;

namespace AlsTools.Infrastructure.Handlers;

/// <summary>
/// The entry point, where basically everything begins...
/// </summary>
public class LiveProjectFileExtractionHandler : ILiveProjectFileExtractionHandler
{
    ILogger<LiveProjectFileExtractionHandler> logger;
    private readonly ILiveProjectsCollectionExtractor liveProjectExtractionHandler;
    private readonly ITracksCollectionExtractor trackExtractionHandler;
    private readonly IScenesCollectionExtractor sceneExtractionHandler;
    private readonly ILocatorsCollectionExtractor locatorExtractionHandler;

    public LiveProjectFileExtractionHandler(ILogger<LiveProjectFileExtractionHandler> logger,
        ILiveProjectsCollectionExtractor liveProjectExtractionHandler,
        ITracksCollectionExtractor trackExtractionHandler,
        IScenesCollectionExtractor sceneExtractionHandler,
        ILocatorsCollectionExtractor locatorExtractionHandler)
    {
        this.logger = logger;
        this.liveProjectExtractionHandler = liveProjectExtractionHandler;
        this.trackExtractionHandler = trackExtractionHandler;
        this.sceneExtractionHandler = sceneExtractionHandler;
        this.locatorExtractionHandler = locatorExtractionHandler;
    }

    public LiveProject ExtractProjectFromFile(string projectFileFullPath)
    {
        logger.LogDebug("=========================================================================");
        logger.LogTrace("Start: ExtractProjectFromFile. File: {@File}", projectFileFullPath);

        logger.LogTrace("Opening project file as read-only...");

        using (FileStream originalFileStream = File.OpenRead(projectFileFullPath))
        {
            logger.LogTrace("Unzipping file into memory...");
            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
            {
                logger.LogTrace("Creating stream reader...");
                using (StreamReader unzip = new StreamReader(decompressionStream))
                {
                    logger.LogTrace("Creating XPathDocument...");
                    var xPathDoc = new XPathDocument(unzip);
                    var nav = xPathDoc.CreateNavigator();

                    logger.LogTrace("Calling the entry point: ExtractProject()...");

                    var project = ExtractProject(Path.GetFileName(projectFileFullPath), projectFileFullPath, nav);
                    return project;
                }
            }
        }
    }

    private LiveProject ExtractProject(string fileName, string fullPath, XPathNavigator nav)
    {
        logger.LogDebug("About to start extracting project data...");
        logger.LogDebug("Project file: {@ProjectFile}", fileName);
        logger.LogDebug("Project path: {@ProjectFullPath}", fullPath);

        // Extract project basic stuff from the project XML
        var project = liveProjectExtractionHandler.ExtractFromXml(nav).Single();
        project.Name = fileName;
        project.Path = fullPath;

        // Extract the scenes
        project.Scenes = sceneExtractionHandler.ExtractFromXml(nav);

        // Extract the locators
        project.Locators = locatorExtractionHandler.ExtractFromXml(nav);

        // Extract all tracks
        project.Tracks = trackExtractionHandler.ExtractFromXml(nav);

        return project;
    }
}
