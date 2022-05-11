using System.Collections.Generic;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Tracks;

namespace AlsTools.Core.Entities
{
    public class LiveProject
    {
        public LiveProject()
        {
            Tracks = new List<ITrack>();
        }

        /// <summary>
        /// Internal (persistency related) project ID
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The project name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The full path of this project
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Creator ("Live version")
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Minor version
        /// </summary>
        public string MinorVersion { get; set; }

        /// <summary>
        /// Major version
        /// </summary>
        public string MajorVersion { get; set; }

        /// <summary>
        /// Schema change count
        /// </summary>
        public int SchemaChangeCount { get; set; }

        /// <summary>
        /// The project tempo/BPM
        /// </summary>
        public int Tempo { get; set; }

        /// <summary>
        /// The project time signature. It is probably a bit mask.
        /// </summary>
        public int TimeSignature { get; set; }

        /// <summary>
        /// The project global groove amount
        /// </summary>
        public int GlobalGrooveAmount { get; set; }

        /// <summary>
        /// The tracks this project contains
        /// </summary>
        public IList<ITrack> Tracks { get; set; } 

        /// <summary>
        /// The scenes this project contains
        /// </summary>
        public IList<Scene> Scenes { get; set; } 

        /// <summary>
        /// The locators this project contains
        /// </summary>
        public IList<Locator> Locators { get; set; } 
    }
}