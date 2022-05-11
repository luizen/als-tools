using System.Collections.Generic;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.ValueObjects.Tracks
{
    public interface ITrack
    {
        /// <summary>
        /// The name the user specified. It can contain special values like # or ##.
        /// Ex.: ## Kick
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// The track effective name, already expanded (if needed).
        /// Ex.: 01 Kick
        /// </summary>
        string EffectiveName { get; set; }

        /// <summary>
        /// The track type. 
        /// <seealso cref="TrackType"/>
        /// </summary>
        TrackType Type { get; set; }

        /// <summary>
        /// The track info text/annotation/notes
        /// </summary>
        string Annotation { get; set; }

        /// <summary>
        /// Whether the track is frozen (Freeze attribute)
        /// </summary>
        bool IsFrozen { get; set; } //TODO: this shouldn't be here, since Master and Return tracks can't be frozen.

        /// <summary>
        /// The group track which this track belongs to, if any.
        /// Can be null. TrackGroupId property.
        /// TODO: this shouldn't be here, since Master and Return tracks can't be grouped.
        /// </summary>
        GroupTrack ParentGroupTrack { get; set; } //TODO: is it really necessary? 

        /// <summary>
        /// Whether or not this track is part of a group track
        /// TODO: this shouldn't be here, since Master and Return tracks can't be grouped.
        /// </summary>
        public bool IsPartOfGroup { get; }

        /// <summary>
        /// The track delay amount, if any
        /// </summary>
        TrackDelay TrackDelay { get; set; }

        /// <summary>
        /// The Ableton Live stock devices this track contains
        /// </summary>
        SortedDictionary<string, LiveDevice> Devices { get; } //TODO: change to another type

        /// <summary>
        /// The third party plugins this track contains
        /// </summary>
        SortedDictionary<string, PluginDevice> Plugins { get; } //TODO: change to another type

        /// <summary>
        /// Adds a device to either the <see cref="Devices" /> or <see cref="Plugins" />
        /// list, only if it does not exist yet.
        /// </summary>
        /// <param name="device">The device object</param>
        void AddDevice(IDevice device);
    }
}