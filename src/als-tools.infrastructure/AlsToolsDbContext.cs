using AlsTools.Core.Models;
using AlsTools.Core.Models.Devices;
using AlsTools.Core.Models.Tracks;
using Microsoft.EntityFrameworkCore;

namespace AlsTools.Infrastructure.Models;

public partial class AlsToolsDbContext : DbContext
{
    public AlsToolsDbContext()
    {
    }

    public AlsToolsDbContext(DbContextOptions<AlsToolsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<BaseTrack> Tracks { get; set; }
    public virtual DbSet<BaseDevice> Devices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=/Users/zenluiz/Documents/Desenvolvimento/repos/als-tools/als-tools-db.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Base Device mapping
        modelBuilder.Entity<BaseDevice>(entity =>
        {
            entity.ToTable("Devices");

            // Id
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnType("INTEGER")
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Name
            entity.Property(e => e.Name)
                .HasColumnType("TEXT")
                .IsRequired();

            // Sort
            entity.Property(e => e.Family.Sort)
                .HasColumnName("DeviceSort")
                .HasColumnType("INTEGER")
                .IsRequired();

            // Type
            entity.Property(e => e.Family.Type)
                .HasColumnName("DeviceType")
                .HasColumnType("INTEGER")
                .IsRequired();

            // fk_TrackId
            entity.Property(e => e.FkTrackId)
                .HasColumnName("fk_TrackId")
                .IsRequired();

            entity.HasOne(d => d.FkTrack).WithMany(p => p.Devices)
                .HasForeignKey(d => d.FkTrackId)
                .OnDelete(DeleteBehavior.Cascade);

            // Device types discriminator
            entity.HasDiscriminator<DeviceType>("Type")
                .HasValue<PluginDevice>(DeviceType.Plugin)
                .HasValue<StockDevice>(DeviceType.Stock)
                .HasValue<MaxForLiveDevice>(DeviceType.MaxForLive);
        });


        // Base Track mapping
        modelBuilder.Entity<BaseTrack>(entity =>
        {
            entity.ToTable("Tracks");

            // Id
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnType("INTEGER")
                .IsRequired()
                .ValueGeneratedOnAdd();

            // UserName
            entity.Property(e => e.UserName)
                .HasColumnType("TEXT")
                .IsRequired();

            // EffectiveName
            entity.Property(e => e.EffectiveName)
                .HasColumnType("TEXT");

            // Type
            entity.Property(e => e.Type)
                .HasColumnType("INTEGER")
                .IsRequired();

            // fk_ProjectId
            entity.Property(e => e.FkProjectId)
                .HasColumnName("fk_ProjectId")
                .IsRequired();

            entity.HasOne(d => d.FkProject).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.FkProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Track types discriminator
            entity.HasDiscriminator<TrackType>("Type")
                .HasValue<AudioTrack>(TrackType.Audio)
                .HasValue<MidiTrack>(TrackType.Midi)
                .HasValue<GroupTrack>(TrackType.Group)
                .HasValue<ReturnTrack>(TrackType.Return)
                .HasValue<MasterTrack>(TrackType.Master);
        });


        // Projects mapping
        modelBuilder.Entity<Project>(entity =>
        {
            // Id
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnType("INTEGER")
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Name
            entity.Property(e => e.Name)
                .HasColumnType("TEXT")
                .IsRequired();

            // Path
            entity.Property(e => e.Path)
                .HasColumnType("TEXT");

            // Tempo
            entity.Property(e => e.Tempo)
                .HasColumnType("INTEGER");

            // TimeSignature
            entity.Property(e => e.TimeSignature)
                .HasColumnType("INTEGER");

            // GlobalGrooveAmount
            entity.Property(e => e.GlobalGrooveAmount)
                .HasColumnType("NUMERIC");

            // SchemaChangeCount
            entity.Property(e => e.SchemaChangeCount)
                .HasColumnType("INTEGER");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
