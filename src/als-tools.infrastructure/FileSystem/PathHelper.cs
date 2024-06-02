public static class PathHelper
{
    /// <summary>
    /// Check if path is a file or not
    /// </summary>
    public static bool IsFile(string path)
    {
        return File.Exists(path);
    }

    /// <summary>
    /// Check if path is a directory or not
    /// </summary>
    public static bool IsDirectory(string path)
    {
        return Directory.Exists(path);
    }
}