namespace AlsTools.Infrastructure.Indexes;

public class LiveProjects_ByPluginNames : AbstractIndexCreationTask<LiveProject, LiveProjects_ByPluginNames.PluginNameResult>
{
    public class PluginNameResult
    {
        public string PluginName { get; set; }
        public PluginFormat PluginFormat { get; set; }
    }

    public LiveProjects_ByPluginNames()
    {
        Map = projects => 
            from project in projects
                from track in project.Tracks
                    from plugin in track.Plugins
                        select new PluginNameResult()
                        {
                            PluginName = plugin.Name,
                            PluginFormat = plugin.Format
                        };

        Index(x => x.PluginName, FieldIndexing.Search);
    }
}
