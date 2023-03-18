namespace AlsTools.Core.Interfaces;

public interface ILiveProjectFileExtractionHandler
{
    LiveProject ExtractProjectFromFile(FileInfo file);
}
