using System;
using System.Collections.Generic;
using AutoMapper;
using LyricsService.Data;
using LyricsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LyricsService.Controllers
{
    [Route("api/l/[controller]")]
    [ApiController]
    public class MusicsController : ControllerBase
    {
        private readonly ILyricRepo _repository;
        private readonly IMapper _mapper;

        public MusicsController(ILyricRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MusicreadDto>> GetAllMusics()
        {
            Console.WriteLine("--> Getting Musics from LyricsService");

            var musicItems = _repository.GetAllMusics();

            return Ok(_mapper.Map<IEnumerable<MusicreadDto>>(musicItems));
        }

       [HttpPost]
       public ActionResult TestInboundConnection()
       {
           Console.WriteLine("--> Inbound POST # Lyric Service");

           return Ok("Inbound test of from Musics Controller");
       }
    }
}
