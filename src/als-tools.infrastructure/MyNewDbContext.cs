using AlsTools.Core.Models;
using AlsTools.Core.ValueObjects.Devices;
using Microsoft.EntityFrameworkCore;

namespace AlsTools.Infrastructure.Models;

public partial class MyNewDbContext : DbContext
{
    public MyNewDbContext()
    {
    }

    public MyNewDbContext(DbContextOptions<MyNewDbContext> options)
        : base(options)
    {
    }

    // public virtual DbSet<Device> Devices { get; set; }

    public DbSet<MyPluginDevice> PluginDevices { get; set; }
    public DbSet<MyStockDevice> StockDevices { get; set; }
    public DbSet<MyMaxForLiveDevice> MaxForLiveDevices { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Track> Tracks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=/Users/zenluiz/Documents/Desenvolvimento/repos/als-tools/als-tools-db.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Base Device mapping
        modelBuilder.Entity<MyBaseDevice>(entity =>
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
            entity.Property(e => e.Sort)
                .HasColumnType("INTEGER")
                .IsRequired();

            // Type
            entity.Property(e => e.Type)
                .HasColumnType("INTEGER")
                .IsRequired();

            // fk_TrackId
            entity.Property(e => e.FkTrackId)
                .HasColumnName("fk_TrackId")
                .IsRequired();

            // Device types discriminator
            entity.HasDiscriminator<DeviceType>("Type")
                .HasValue<MyPluginDevice>(DeviceType.Plugin)
                .HasValue<MyStockDevice>(DeviceType.Stock)
                .HasValue<MyMaxForLiveDevice>(DeviceType.MaxForLive);
        });

        // PluginDevice
        modelBuilder.Entity<MyPluginDevice>(entity =>
        {
            entity.HasOne(d => d.FkTrack).WithMany(p => p.PluginDevices)
                .HasForeignKey(d => d.FkTrackId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // StockDevice
        modelBuilder.Entity<MyStockDevice>(entity =>
        {
            entity.HasOne(d => d.FkTrack).WithMany(p => p.StockDevices)
                .HasForeignKey(d => d.FkTrackId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // MaxForLiveDevice
        modelBuilder.Entity<MyMaxForLiveDevice>(entity =>
        {
            entity.HasOne(d => d.FkTrack).WithMany(p => p.MaxForLiveDevices)
                .HasForeignKey(d => d.FkTrackId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Tracls mapping
        modelBuilder.Entity<Track>(entity =>
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
