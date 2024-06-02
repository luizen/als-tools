namespace AlsTools.Core.Config;

public class InitDbOptions
{
    public string[] Folders { get; set; } = [];
    public string[] Files { get; set; } = [];
    public bool IncludeBackups { get; set; }
}