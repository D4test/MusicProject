using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicService.AsyncDataServices;
using MusicService.Data;
using MusicService.Dto;
using MusicService.Dtos;
using MusicService.Models;
using MusicService.SyncDataServices.Http;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicsController : ControllerBase
    {
        private readonly IMusicRepo _repository;
        private readonly IMapper _mapper;
        private readonly ILyricDataClient _lyricDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public MusicsController(
            IMusicRepo repository, 
            IMapper mapper,
            ILyricDataClient lyricDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _lyricDataClient = lyricDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MusicReadDto>> GetMusics()
        {
            Console.WriteLine("--> Getting Musics....");

            var musicItem = _repository.GetAllMusics();

            return Ok(_mapper.Map<IEnumerable<MusicReadDto>>(musicItem));
        }

        [HttpGet("{id}", Name = "GetMusicById")]
        public ActionResult<MusicReadDto> GetMusicById(int id)
        {
            var musicItem = _repository.GetMusicById(id);
            if (musicItem != null)
            {
                return Ok(_mapper.Map<MusicReadDto>(musicItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<MusicReadDto>> CreateMusic(MusicCreateDto musicCreateDto)
        {
            var musicModel = _mapper.Map<Music>(musicCreateDto);
            _repository.CreateMusic(musicModel);
            _repository.SaveChanges();

            var musicReadDto = _mapper.Map<MusicReadDto>(musicModel);

            // Send Sync Message
            try
            {
                await _lyricDataClient.SendMusicToLyric(musicReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }
            //Send Async Message
            try
            {
                var musicPublishedDto = _mapper.Map<MusicPublishedDto>(musicReadDto);
                musicPublishedDto.Event = "Music_Published";
                _messageBusClient.PublishNewMusic(musicPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetMusicById), new { Id = musicReadDto.Id}, musicReadDto);
        }
    }
}