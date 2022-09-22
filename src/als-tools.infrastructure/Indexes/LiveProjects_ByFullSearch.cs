using AlsTools.Core.Entities;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class LiveProjects_ByFullSearch : AbstractIndexCreationTask<LiveProject>
{
    public LiveProjects_ByFullSearch()
    {
        Map = projects =>
            from project in projects
            from track in project.Tracks
            select new
            {
                ProjectName = project.Name,
                ProjectCreator = project.Creator,
                ProjectMinorVersion = project.MinorVersion,
                ProjectMajorVersion = project.MajorVersion,
                ProjectTempo = project.Tempo,

                SceneNames = project.Scenes.Select(s => s.Name),
                SceneNumbers = project.Scenes.Select(s => s.Number),
                SceneTempos = project.Scenes.Select(s => s.Tempo),
                SceneAnnotations = project.Scenes.Select(s => s.Annotation),
                SceneIsTempoEnableds = project.Scenes.Select(s => s.IsTempoEnabled),
                SceneIsTimeSignatureEnableds = project.Scenes.Select(s => s.IsTimeSignatureEnabled),

                LocatorNames = project.Locators.Select(l => l.Name),
                LocatorNumbers = project.Locators.Select(l => l.Number),
                LocatorAnnotations = project.Locators.Select(l => l.Annotation),
                LocatorTimes = project.Locators.Select(l => l.Time),
                LocatorIsSongStarts = project.Locators.Select(l => l.IsSongStart),

                TrackUserNames = project.Tracks.Select(t => t.UserName),
                TrackEffectiveNames = project.Tracks.Select(t => t.EffectiveName),
                TrackIsFrozens = project.Tracks.Select(t => t.IsFrozen),
                TrackIsPartOfGroups = project.Tracks.Select(t => t.IsPartOfGroup),
                TrackTypes = project.Tracks.Select(t => t.Type),
                TrackAnnotations = project.Tracks.Select(t => t.Annotation),

                StockDeviceNames = track.StockDevices.Select(sd => sd.Name),
                StockDeviceUserNames = track.StockDevices.Select(sd => sd.UserName),
                StockDeviceAnnotations = track.StockDevices.Select(sd => sd.Annotation),
                StockDeviceSorts = track.StockDevices.Select(sd => sd.Family.Sort),

                MaxForLiveDeviceNames = track.MaxForLiveDevices.Select(mld => mld.Name),
                MaxForLiveDeviceUserNames = track.MaxForLiveDevices.Select(mld => mld.UserName),
                MaxForLiveDeviceAnnotations = track.MaxForLiveDevices.Select(mld => mld.Annotation),
                MaxForLiveDeviceSorts = track.MaxForLiveDevices.Select(mld => mld.Family.Sort),

                Tracks_Plugins_Name = track.Plugins.Select(p => p.Name),
                Tracks_Plugins_UserName = track.Plugins.Select(p => p.UserName),
                Tracks_Plugins_Format = track.Plugins.Select(p => p.Format),
                Tracks_Plugins_Sort = track.Plugins.Select(p => p.Family.Sort),
                Tracks_Plugins_Annotation = track.Plugins.Select(p => p.Annotation)
            };
    }
}
