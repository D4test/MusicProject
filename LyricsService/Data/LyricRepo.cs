using System;
using System.Collections.Generic;
using System.Linq;
using LyricsService.Models;


namespace LyricsService.Data
{
    public class LyricRepo : ILyricRepo
    {
        private readonly AppDbContext _context;

        public LyricRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateLyric(int musicId, Lyric lyric)
        {
            if (lyric == null)
            {
                throw new ArgumentNullException(nameof(lyric));
            }

            lyric.MusicId = musicId;
            _context.Lyrics.Add(lyric);
        }

        public void CreateMusic(Music plat)
        {
            if(plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            _context.Musics.Add(plat);
        }

        public bool ExternalMusicExists(int externalMusicId)
        {
            return _context.Musics.Any(p => p.ExternalID == externalMusicId);
        }

        public IEnumerable<Music> GetAllMusics()
        {
            return _context.Musics.ToList();
        }

        public Lyric GetLyric(int musicId, int lyricId)
        {
            return _context.Lyrics
                .Where(c => c.MusicId == musicId && c.Id == lyricId).FirstOrDefault();
        }

        public IEnumerable<Lyric> GetLyricsForMusic(int musicId)
        {
            return _context.Lyrics
                .Where(c => c.MusicId == musicId)
                .OrderBy(c => c.Music.Name);
        }

        public bool MusicExits(int musicId)
        {
            return _context.Musics.Any(p => p.Id == musicId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}