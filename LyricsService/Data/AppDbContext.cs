using LyricsService.Models;
using Microsoft.EntityFrameworkCore;


namespace LyricsService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt ) : base(opt)
        {
            
        }

        public DbSet<Music> Musics { get; set; }
        public DbSet<Lyric> Lyrics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Music>()
                .HasMany(p => p.Lyrics)
                .WithOne(p=> p.Music!)
                .HasForeignKey(p => p.MusicId);

            modelBuilder
                .Entity<Lyric>()
                .HasOne(p => p.Music)
                .WithMany(p => p.Lyrics)
                .HasForeignKey(p =>p.MusicId);
        }
    }
}