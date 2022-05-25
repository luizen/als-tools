namespace AlsTools.Infrastructure.FileSystem;

public class UserFolderHandler
{
    private readonly string userHomeFolder;

    public UserFolderHandler(string userHomeFolder)
    {
        this.userHomeFolder = userHomeFolder;
    }
    
    /// <summary>
    /// In *nix-like systems (linux and macOS, for instance), a path starting with "~/" indicates it is under the user home directory.
    /// In these cases, this method returns the expanded, full path of the folder or file passed in the <paramref name="path"/> parameter.
    /// </summary>
    /// <param name="path">The original path to get the correct full path for</param>
    /// <returns>If the path does not start with "~/", then it returns the original <paramref name="path"/>. Otherwise, it returns the expanded, full path.</returns>
    public string GetFullPath(string path)
    {
        if (!path.StartsWith("~/"))
            return path;

        var pathWithoutTildeSlash = path.Substring(2);

        return Path.Combine(userHomeFolder, pathWithoutTildeSlash);
    }
}