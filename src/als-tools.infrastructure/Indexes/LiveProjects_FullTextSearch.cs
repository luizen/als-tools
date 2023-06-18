namespace AlsTools.Infrastructure.Indexes;

public class LiveProjects_FullTextSearch : AbstractIndexCreationTask<LiveProject, LiveProjects_FullTextSearch.Result>
{
    public class Result
    {
        public object Query;
    }

    public LiveProjects_FullTextSearch()
    {
        Map = projects =>
            from project in projects
            from track in project.Tracks
            select new Result()
            {
                Query = new object[]
                {
                    project.Name,
                    project.Creator,
                    project.MinorVersion,
                    project.MajorVersion,
                    project.Tempo,
                    project.Scenes.Select(s => s.Name),
                    project.Scenes.Select(s => s.Number),
                    project.Scenes.Select(s => s.Tempo),
                    project.Scenes.Select(s => s.Annotation),
                    project.Scenes.Select(s => s.IsTempoEnabled),
                    project.Scenes.Select(s => s.IsTimeSignatureEnabled),

                    project.Locators.Select(l => l.Name),
                    project.Locators.Select(l => l.Number),
                    project.Locators.Select(l => l.Annotation),
                    project.Locators.Select(l => l.Time),
                    project.Locators.Select(l => l.IsSongStart),

                    project.Tracks.Select(t => t.UserName),
                    project.Tracks.Select(t => t.EffectiveName),
                    project.Tracks.Select(t => t.IsFrozen),
                    project.Tracks.Select(t => t.IsPartOfGroup),
                    project.Tracks.Select(t => t.Type),
                    project.Tracks.Select(t => t.Annotation),

                    track.StockDevices.Select(sd => sd.Name),
                    track.StockDevices.Select(sd => sd.UserName),
                    track.StockDevices.Select(sd => sd.Annotation),
                    track.StockDevices.Select(sd => sd.Family.Sort),


                    // track.StockDevices.Select(sd => LoadDocument<Device.Family.Sort),

                    track.MaxForLiveDevices.Select(mld => mld.Name),
                    track.MaxForLiveDevices.Select(mld => mld.UserName),
                    track.MaxForLiveDevices.Select(mld => mld.Annotation),
                    track.MaxForLiveDevices.Select(mld => mld.Family.Sort),

                    track.Plugins.Select(p => p.Name),
                    track.Plugins.Select(p => p.UserName),
                    track.Plugins.Select(p => p.Format),
                    track.Plugins.Select(p => p.Family.Sort),
                    track.Plugins.Select(p => p.Annotation)
                }
            };

        Index("Query", FieldIndexing.Search);
    }
}