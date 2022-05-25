using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using AlsTools.Infrastructure.Handlers;

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
        logger.LogDebug("=========================================================================");
        logger.LogTrace("Start: ExtractProjectFromFile. File: {@File}", file.FullName);

        logger.LogTrace("Opening project file as read-only...");
        using (FileStream originalFileStream = file.OpenRead())
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
                    var project = ExtractProject(file.Name, file.FullName, nav);
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

        var project = liveProjectExtractionHandler.ExtractFromXml(nav).Single();
        project.Name = fileName;
        project.Path = fullPath;
        project.Scenes = sceneExtractionHandler.ExtractFromXml(nav);
        project.Locators = locatorExtractionHandler.ExtractFromXml(nav);
        project.Tracks = trackExtractionHandler.ExtractFromXml(nav);

        return project;
    }
}
