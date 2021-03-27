using System.Collections.Generic;

namespace AlsTools.Core.Entities
{
    public class LiveProject
    {
        public LiveProject()
        {
            Plugins = new SortedDictionary<string, PluginInfo>();
        }

        public string Name { get; set; }

        public string Path { get; set; }

        public string LiveVersion { get; set; }

        public SortedDictionary<string, PluginInfo> Plugins { get; set; }
    }
}