using System.IO;
using AlsTools.Core.Entities;

namespace AlsTools.Core.Interfaces
{
    public interface ILiveProjectExtractor
    {
        LiveProject ExtractProjectFromFile(FileInfo file);
    }
}
