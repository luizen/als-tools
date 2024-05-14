using AlsTools.Core.Models.Devices;

namespace AlsTools.Core.Models.Tracks;

public abstract class BaseTrack
{
    public const int DefaultGroupId = -1;

    private const int UndefinedTrackGroupId = -1;

    public BaseTrack(TrackType type)
    {
        Type = type;
    }

    /// <summary>
    /// Internal (persistency related) ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The track intenal Id attribute. Might be not available (eg.: Master Track)
    /// </summary>
    public int? TrackId { get; set; }

    /// <summary>
    /// The name the user specified. It can contain special values like # or ##.
    /// Ex.: ## Kick
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The track effective name, already expanded (if needed).
    /// Ex.: 01 Kick
    /// </summary>
    public string? EffectiveName { get; set; }

    /// <summary>
    /// The track type.
    /// <seealso cref="TrackType"/>
    /// </summary>
    public TrackType Type { get; set; }

    /// <summary>
    /// The track info text/annotation/notes
    /// </summary>
    public string Annotation { get; set; } = string.Empty;

    /// <summary>
    /// Whether the track is frozen (Freeze attribute)
    /// </summary>
    public bool? IsFrozen { get; set; } //TODO: this shouldn't be here, since Master and Return tracks can't be frozen.

    /// <summary>
    /// The group track Id which this track belongs to, if any.
    /// Can be null? TrackGroupId property.
    /// TODO: this shouldn't be here, since Master and Return tracks can't be grouped.
    /// </summary>
    public int TrackGroupId { get; set; } //TODO: is it really necessary?

    /// <summary>
    /// Whether or not this track is part of a group track
    /// TODO: this shouldn't be here, since Master and Return tracks can't be grouped.
    /// </summary>
    public bool IsPartOfGroup => TrackGroupId != UndefinedTrackGroupId;

    /// <summary>
    /// The track delay amount, if any
    /// </summary>
    public TrackDelay TrackDelay { get; set; } = new TrackDelay();

    /// <summary>
    /// The devices this track contains
    /// </summary>
    public virtual ICollection<BaseDevice> Devices { get; set; } = new List<BaseDevice>();

    /// <summary>
    /// Track color
    /// </summary>
    public LiveColor Color { get; set; } = LiveColors.Unset; // TODO: should I use a nullable LiveColor?

    /// <summary>
    /// Whether the track is muted or not
    /// </summary>
    public bool? IsMuted { get; set; }

    /// <summary>
    /// Wehther the track is soloed or not
    /// </summary>
    public bool? IsSoloed { get; set; }

    /// <summary>
    /// Helper property that tells whether this track is a Group track
    /// </summary>
    public bool IsGroupTrack => Type == TrackType.Group;

    /// <summary>
    /// Helper property that tells whether this track is a Audio track
    /// </summary>
    public bool IsAudioTrack => Type == TrackType.Audio;

    /// <summary>
    /// Helper property that tells whether this track is a Midi track
    /// </summary>
    bool IsMidiTrack => Type == TrackType.Midi;

    /// <summary>
    /// Helper property that tells whether this track is a Return track
    /// </summary>
    public bool IsReturnGroupTrack => Type == TrackType.Return;

    /// <summary>
    /// Helper property that tells whether this track is a Master track
    /// </summary>
    public bool IsMasterTrack => Type == TrackType.Master;

    /// <summary>
    /// The foreign key to the project this track belongs to.
    /// </summary>
    public int FkProjectId { get; set; }

    /// <summary>
    /// The project this track belongs to.
    /// </summary>
    public virtual Project FkProject { get; set; } = null!;

    /// <summary>
    /// Adds a device to either the <see cref="StockDevices" />, <see cref="Plugins" /> or <see cref="MaxForLiveDevices" />
    /// list. Duplicated entries are allowed.
    /// </summary>
    /// <param name="device">The device object</param>
    public void AddDevice(BaseDevice device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device));

        Devices.Add(device);
    }

    /// <summary>
    /// Adds a list of devices to either the <see cref="StockDevices" />, <see cref="Plugins" /> or <see cref="MaxForLiveDevices" />
    /// list. Duplicated entries are allowed.
    /// </summary>
    /// <param name="devices">The list of device objects</param>
    public void AddDevices(IEnumerable<BaseDevice> devices)
    {
        foreach (var device in devices)
            AddDevice(device);
    }
}