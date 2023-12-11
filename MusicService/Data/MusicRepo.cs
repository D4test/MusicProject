using System;
using System.Collections.Generic;
using System.Linq;
using MusicService.Models;

namespace MusicService.Data
{
    public class MusicRepo : IMusicRepo
    {
        private readonly AppDbContext _context;

        public MusicRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateMusic(Music plat)
        {
            if(plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }

            _context.Musics.Add(plat);
        }

        public IEnumerable<Music> GetAllMusics()
        {
            return _context.Musics.ToList();
        }

        public Music GetMusicById(int id)
        {
            return _context.Musics.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}