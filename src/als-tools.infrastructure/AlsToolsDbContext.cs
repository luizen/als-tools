
// using AlsTools.Core.Entities;
// using AlsTools.Core.ValueObjects.Devices;
// using AlsTools.Core.ValueObjects.Tracks;
// using Microsoft.EntityFrameworkCore;

// public class AlsToolsContext : DbContext
// {
//     public DbSet<LiveProject> LiveProjects { get; set; }
//     public DbSet<ITrack> Tracks { get; set; }
//     public DbSet<PluginDevice> Plugins { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     {
//         optionsBuilder.UseSqlite("Data Source=als-tools.db");
//     }
//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder.Entity<LiveProject>().HasKey(x => x.Id);
//         modelBuilder.Entity<LiveProject>().HasMany(x => x.Tracks).WithOne(x => x.LiveProject).HasForeignKey(x => x.LiveProjectId);

//         modelBuilder.Entity<ITrack>().HasKey(x => x.Id);
//         modelBuilder.Entity<ITrack>().HasMany(x => x.Plugins).WithOne(x => x.Track).HasForeignKey(x => x.TrackId);

//         modelBuilder.Entity<PluginDevice>().HasKey(x => x.Id);
//     }
// }