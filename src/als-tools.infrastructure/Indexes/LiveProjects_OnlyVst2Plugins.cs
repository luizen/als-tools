namespace AlsTools.Infrastructure.Indexes;

public class LiveProjects_OnlyVst2Plugins : AbstractIndexCreationTask<LiveProject>
{
    public LiveProjects_OnlyVst2Plugins()
    {
        Map = projects => 
            from project in projects
                from track in project.Tracks
                    from plugin in track.Plugins
                    where plugin.Format == PluginFormat.VST2
                        select new 
                        {
                            PluginName = plugin.Name,
                            PluginFormat = plugin.Format
                        };
    }
}
