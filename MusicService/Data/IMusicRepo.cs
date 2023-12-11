using System.Collections.Generic;
using MusicService.Models;

namespace MusicService.Data
{
    public interface IMusicRepo
    {
        bool SaveChanges();

        IEnumerable<Music> GetAllMusics();
        Music GetMusicById(int id);
        void CreateMusic(Music plat);
    }
}
