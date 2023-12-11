using System;
using System.Collections.Generic;
using AutoMapper;
using LyricsService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using MusicService;

namespace LyricsService.SyncDataServices.Grpc
{
    public class MusicDataClient : IMusicDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MusicDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Music> ReturnAllMusics()
        {
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcMusic"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcMusic"]);
            var client = new GrpcMusic.GrpcMusicClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllMusics(request);
                return _mapper.Map<IEnumerable<Music>>(reply.Music);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}