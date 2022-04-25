

using System.Linq;
using AlsTools.Core.Entities;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes
{
    public class LiveProjects_ByPluginNames : AbstractIndexCreationTask<LiveProject>
    {
        public LiveProjects_ByPluginNames()
        {
            Map = projects => 
                from project in projects
                    from track in project.Tracks
                        from plugin in track.Plugins
                            select new
                            {
                                plugin.Value.Name
                            };
        }
    }
}
