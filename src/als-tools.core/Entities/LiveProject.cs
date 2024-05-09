using System.ComponentModel.DataAnnotations;
using AlsTools.Core.ValueObjects.Tracks;

namespace AlsTools.Core.Entities;

public class LiveProject
{
    public LiveProject()
    {
        Tracks = [];
    }

    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public IList<ITrack> Tracks { get; set; }
}