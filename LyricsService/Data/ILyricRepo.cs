using System.Collections.Generic;
using LyricsService.Models;

namespace LyricsService.Data
{
    public interface ILyricRepo
    {
        bool SaveChanges();

        // Musics
        IEnumerable<Music> GetAllMusics();
        void CreateMusic(Music plat);
        bool MusicExits(int musicId);
        bool ExternalMusicExists(int externalMusicId);

        // Lyrics
        IEnumerable<Lyric> GetLyricsForMusic(int musicId);
        Lyric GetLyric(int musicId, int lyricId);
        void CreateLyric(int musicId, Lyric lyric);
    }
}