using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectFileExtractionHandler
{
    LiveProject ExtractProjectFromFile(string projectFileFullPath);
}
