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
            select new //ProjectResult()
            {
                // ProjectName = project.Name,  // --> THIS DOES NOT WORK, IT GIVES ERROR SAYING THAT THIS PROPERTY IS NOT INDEXED.
                Name = project.Name, // --> THIS WORKS.
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


                // StockDeviceSorts = track.StockDevices.Select(sd => LoadDocument<Device.Family.Sort),



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



// public class ProjectResult
// {
//     public string ProjectName { get; set; } = string.Empty;
//     public string ProjectCreator { get; set; } = string.Empty;
//     public string ProjectMinorVersion { get; set; } = string.Empty;
//     public string ProjectMajorVersion { get; set; } = string.Empty;
//     public double ProjectTempo { get; set; }

//     public IEnumerable<string> SceneNames { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<int> SceneNumbers { get; set; } = Enumerable.Empty<int>();
//     public IEnumerable<int> SceneTempos { get; set; } = Enumerable.Empty<int>();
//     public IEnumerable<string> SceneAnnotations { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<bool> SceneIsTempoEnableds { get; set; } = Enumerable.Empty<bool>();
//     public IEnumerable<bool> SceneIsTimeSignatureEnableds { get; set; } = Enumerable.Empty<bool>();

//     public IEnumerable<string> LocatorNames { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<int?> LocatorNumbers { get; set; } = Enumerable.Empty<int?>();
//     public IEnumerable<string> LocatorAnnotations { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<double> LocatorTimes { get; set; } = Enumerable.Empty<double>();
//     public IEnumerable<bool> LocatorIsSongStarts { get; set; } = Enumerable.Empty<bool>();

//     public IEnumerable<string> TrackUserNames { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<string?> TrackEffectiveNames { get; set; } = Enumerable.Empty<string?>();
//     public IEnumerable<bool?> TrackIsFrozens { get; set; } = Enumerable.Empty<bool?>();
//     public IEnumerable<bool> TrackIsPartOfGroups { get; set; } = Enumerable.Empty<bool>();
//     public IEnumerable<TrackType> TrackTypes { get; set; } = Enumerable.Empty<TrackType>();
//     public IEnumerable<string> TrackAnnotations { get; set; } = Enumerable.Empty<string>();

//     public IEnumerable<string> StockDeviceNames { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<string> StockDeviceUserNames { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<string> StockDeviceAnnotations { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<DeviceSort> StockDeviceSorts { get; set; } = Enumerable.Empty<DeviceSort>();


//     public IEnumerable<string> MaxForLiveDeviceNames { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<string> MaxForLiveDeviceUserNames { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<string> MaxForLiveDeviceAnnotations { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<DeviceSort> MaxForLiveDeviceSorts { get; set; } = Enumerable.Empty<DeviceSort>();

//     public IEnumerable<string> Tracks_Plugins_Name { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<string> Tracks_Plugins_UserName { get; set; } = Enumerable.Empty<string>();
//     public IEnumerable<PluginFormat> Tracks_Plugins_Format { get; set; } = Enumerable.Empty<PluginFormat>();
//     public IEnumerable<DeviceSort> Tracks_Plugins_Sort { get; set; } = Enumerable.Empty<DeviceSort>();
//     public IEnumerable<string> Tracks_Plugins_Annotation { get; set; } = Enumerable.Empty<string>();

// }


// public class LiveProjects_ByFullSearch : AbstractIndexCreationTask<LiveProject>
// {
//     public LiveProjects_ByFullSearch()
//     {
//         Map = projects =>
//             from project in projects
//             from track in project.Tracks
//             select new
//             {
//                 ProjectName = project.Name,
//                 ProjectCreator = project.Creator,
//                 ProjectMinorVersion = project.MinorVersion,
//                 ProjectMajorVersion = project.MajorVersion,
//                 ProjectTempo = project.Tempo,

//                 SceneNames = project.Scenes.Select(s => s.Name),
//                 SceneNumbers = project.Scenes.Select(s => s.Number),
//                 SceneTempos = project.Scenes.Select(s => s.Tempo),
//                 SceneAnnotations = project.Scenes.Select(s => s.Annotation),
//                 SceneIsTempoEnableds = project.Scenes.Select(s => s.IsTempoEnabled),
//                 SceneIsTimeSignatureEnableds = project.Scenes.Select(s => s.IsTimeSignatureEnabled),

//                 LocatorNames = project.Locators.Select(l => l.Name),
//                 LocatorNumbers = project.Locators.Select(l => l.Number),
//                 LocatorAnnotations = project.Locators.Select(l => l.Annotation),
//                 LocatorTimes = project.Locators.Select(l => l.Time),
//                 LocatorIsSongStarts = project.Locators.Select(l => l.IsSongStart),

//                 TrackUserNames = project.Tracks.Select(t => t.UserName),
//                 TrackEffectiveNames = project.Tracks.Select(t => t.EffectiveName),
//                 TrackIsFrozens = project.Tracks.Select(t => t.IsFrozen),
//                 TrackIsPartOfGroups = project.Tracks.Select(t => t.IsPartOfGroup),
//                 TrackTypes = project.Tracks.Select(t => t.Type),
//                 TrackAnnotations = project.Tracks.Select(t => t.Annotation),

//                 StockDeviceNames = track.StockDevices.Select(sd => sd.Name),
//                 StockDeviceUserNames = track.StockDevices.Select(sd => sd.UserName),
//                 StockDeviceAnnotations = track.StockDevices.Select(sd => sd.Annotation),
//                 StockDeviceSorts = track.StockDevices.Select(sd => sd.Family.Sort),


//                 // StockDeviceSorts = track.StockDevices.Select(sd => LoadDocument<Device.Family.Sort),



//                 MaxForLiveDeviceNames = track.MaxForLiveDevices.Select(mld => mld.Name),
//                 MaxForLiveDeviceUserNames = track.MaxForLiveDevices.Select(mld => mld.UserName),
//                 MaxForLiveDeviceAnnotations = track.MaxForLiveDevices.Select(mld => mld.Annotation),
//                 MaxForLiveDeviceSorts = track.MaxForLiveDevices.Select(mld => mld.Family.Sort),

//                 Tracks_Plugins_Name = track.Plugins.Select(p => p.Name),
//                 Tracks_Plugins_UserName = track.Plugins.Select(p => p.UserName),
//                 Tracks_Plugins_Format = track.Plugins.Select(p => p.Format),
//                 Tracks_Plugins_Sort = track.Plugins.Select(p => p.Family.Sort),
//                 Tracks_Plugins_Annotation = track.Plugins.Select(p => p.Annotation)
//             };
//     }
// }
