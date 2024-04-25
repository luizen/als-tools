using Microsoft.EntityFrameworkCore;
using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.Tracks;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure
{
    public class AlsToolsDbContext : DbContext
    {
        public DbSet<LiveProject> LiveProjects { get; set; }
        public DbSet<ITrack> Tracks { get; set; }
        public DbSet<IDevice> Devices { get; set; }
        // public DbSet<PluginDevice> Plugins { get; set; }
        // public DbSet<MaxForLiveDevice> MaxForLiveDevices { get; set; }
        public DbSet<Scene> Scenes { get; set; }
        public DbSet<Locator> Locators { get; set; }

        public AlsToolsDbContext(DbContextOptions<AlsToolsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LiveProject>(proj =>
            {
                proj.HasMany(p => p.Tracks)
                    .WithOne(t => t.LiveProject)
                    .HasForeignKey(t => t.LiveProjectId);

                proj.HasMany(p => p.Scenes)
                    .WithOne(s => s.LiveProject)
                    .HasForeignKey(s => s.LiveProjectId);

                proj.HasMany(p => p.Locators)
                    .WithOne(l => l.LiveProject)
                    .HasForeignKey(l => l.LiveProjectId);
            });



            modelBuilder.Entity<BaseTrack>(track =>
            {
                track.HasMany(t => t.StockDevices)
                                .WithOne(s => s.Track)
                                .HasForeignKey(d => d.TrackId);
            });


            modelBuilder.Entity<Track>()
                .HasMany(t => t.Plugins)
                .WithOne()
                .HasForeignKey(d => d.TrackId);

            modelBuilder.Entity<Track>()
                .HasMany(t => t.MaxForLiveDevices)
                .WithOne()
                .HasForeignKey(d => d.TrackId);

            modelBuilder.Entity<LiveProject>()
                .HasMany(p => p.Scenes)
                .WithOne(s => s.LiveProject)
                .HasForeignKey(s => s.LiveProjectId);

            modelBuilder.Entity<LiveProject>()
                .HasMany(p => p.Locators)
                .WithOne(l => l.LiveProject)
                .HasForeignKey(l => l.LiveProjectId);
        }
    }
}