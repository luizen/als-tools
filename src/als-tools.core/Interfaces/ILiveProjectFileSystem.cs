using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces;

public interface ILiveProjectFileSystem
{
    IEnumerable<string> GetProjectFilesFullPathFromPaths(IEnumerable<string> paths, bool includeBackupFolder = false);

    void SetFileDates(LiveProject project);
}
