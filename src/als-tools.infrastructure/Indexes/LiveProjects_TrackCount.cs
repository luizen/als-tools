using AlsTools.Core;
using AlsTools.Core.Entities;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure;

public class LiveProjects_TrackCount : AbstractIndexCreationTask<LiveProject>
{
    public LiveProjects_TrackCount()
    {
        // Map = projects =>
        //      (from project in projects
        //       select new NameCountElement()
        //       {
        //           Name = project.Name,
        //           Count = project.Tracks.Count
        //       })
        //       .OrderByDescending(x => x.Count)
        //       .Take(10);

        Map = projects =>
             from project in projects
             select new
             {
                 Name = project.Name,
                 Count = project.Tracks.Count
             };

        Reduce = results => from result in results
                            group result by result.Name into g
                            select new
                            {
                                Name = g.Key,
                                Count = g.Sum(x => x.Tracks.Count())
                            };
    }
}
