namespace AlsTools.Core.Config;

/// <summary>
/// Database options, to be configured via appsettings.json file
/// </summary>
public class DbOptions
{
    /// <summary>
    /// The folder where the database data files will be created
    /// </summary>
    public string DataLocation { get; set; }

    /// <summary>
    /// The database server URL to connect to
    /// </summary>
    public string ServerUrl { get; set; }

    /// <summary>
    /// The root document store name, where data will be saved to
    /// </summary>
    public string DocumentStoreName { get; set; }
}
