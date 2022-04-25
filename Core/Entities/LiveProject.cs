using System;
using System.Collections.Generic;
using AlsTools.Core.Entities.Tracks;

namespace AlsTools.Core.Entities
{
    public class LiveProject
    {
        public LiveProject()
        {
            Tracks = new List<ITrack>();
        }

        public string Id { get; set; }
        
        public string Name { get; set; }

        public string Path { get; set; }

        public string LiveVersion { get; set; }

        public IList<ITrack> Tracks { get; set; } 
    }
}