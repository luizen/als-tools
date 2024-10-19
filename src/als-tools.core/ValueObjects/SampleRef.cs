namespace AlsTools.Core.ValueObjects;

public class SampleRef
{
    public required string FileRefPath { get; set; }
    // public required FileRef FileRef { get; set; }
    // public long LastModDate { get; set; }
    // public SourceContext? SourceContext { get; set; }
    // public int SampleUsageHint { get; set; }
    // public int DefaultDuration { get; set; }
    // public int DefaultSampleRate { get; set; }
}

// public class FileRef
// {
// public int RelativePathType { get; set; }
// public string RelativePath { get; set; }
// public string Path { get; set; }
// public int Type { get; set; }
// public string LivePackName { get; set; }
// public string LivePackId { get; set; }
// public long OriginalFileSize { get; set; }
// public int OriginalCrc { get; set; }
// }

// public class SourceContext
// {
//     public int Id { get; set; }
//     public OriginalFileRef OriginalFileRef { get; set; }
//     public string BrowserContentPath { get; set; }
//     public string LocalFiltersJson { get; set; }
// }
//
// public class OriginalFileRef
// {
//     public FileRef FileRef { get; set; }
// }
