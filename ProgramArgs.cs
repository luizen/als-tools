namespace AlsTools
{
    public class ProgramArgs
    {
        public ProgramArgs()
        {
            PluginsToLocate = new string[0];
        }

        public string Folder { get; set; }

        public string File { get; set; }

        public bool LocatePlugins { get; set; }

        public bool ListPlugins { get; set; }

        public string[] PluginsToLocate { get; set; }

        public bool IncludeBackups { get; set; }

        public bool InitDb { get; set; }

        public bool CountProjects { get; set; }

        public bool Export { get; set; }

        public string ExportFile { get; set; }
    }
}

