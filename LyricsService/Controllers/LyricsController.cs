using System;
using System.Collections.Generic;
using AutoMapper;
using LyricsService.Data;
using LyricsService.Dtos;
using LyricsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace LyricsService.Controllers
{
    [Route("api/l/musics/{musicId}/[controller]")]
    [ApiController]
    public class LyricsController : ControllerBase
    {
        private readonly ILyricRepo _repository;
        private readonly IMapper _mapper;

        public LyricsController(ILyricRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<LyricReadDto>> GetLyricsForMusic(int musicId)
        {
            Console.WriteLine($"--> Hit GetLyricsForMusic: {musicId}");

            if (!_repository.MusicExits(musicId))
            {
                return NotFound();
            }

            var lyrics = _repository.GetLyricsForMusic(musicId);

            return Ok(_mapper.Map<IEnumerable<LyricReadDto>>(lyrics));
        }

        [HttpGet("{lyricId}", Name = "GetLyricForMusic")]
        public ActionResult<LyricReadDto> GetLyricForMusic(int musicId, int lyricId)
        {
            Console.WriteLine($"--> Hit GetLyricForMusic: {musicId} / {lyricId}");

            if (!_repository.MusicExits(musicId))
            {
                return NotFound();
            }

            var lyric = _repository.GetLyric(musicId, lyricId);

            if(lyric == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<LyricReadDto>(lyric));
        }

        [HttpPost]
        public ActionResult<LyricReadDto> CreateLyricForMusic(int musicId, LyricCreateDto lyricDto)
        {
             Console.WriteLine($"--> Hit CreateLyricForMusic: {musicId}");

            if (!_repository.MusicExits(musicId))
            {
                return NotFound();
            }

            var lyric = _mapper.Map<Lyric>(lyricDto);

            _repository.CreateLyric(musicId, lyric);
            _repository.SaveChanges();

            var lyricReadDto = _mapper.Map<LyricReadDto>(lyric);

            return CreatedAtRoute(nameof(GetLyricForMusic),
                new {musicId = musicId, lyricId = lyricReadDto.Id}, lyricReadDto);
        }

    }
}