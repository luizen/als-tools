using AlsTools.Core.Models;
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

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=/Users/zenluiz/Documents/Desenvolvimento/repos/als-tools/als-tools-db.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(entity =>
        {
            entity.Property(e => e.FkTrackId).HasColumnName("fk_TrackId");

            entity.HasOne(d => d.FkTrack).WithMany(p => p.Devices)
                .HasForeignKey(d => d.FkTrackId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FkProjectId).HasColumnName("fk_ProjectId");

            entity.HasOne(d => d.FkProject).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.FkProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
