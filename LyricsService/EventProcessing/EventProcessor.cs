using System;
using System.Text.Json;
using AutoMapper;
using LyricsService.Data;
using LyricsService.Dtos;
using LyricsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LyricsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.MusicPublished:
                    addMusic(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notifcationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

            switch(eventType.Event)
            {
                case "Music_Published":
                    Console.WriteLine("--> Music Published Event Detected");
                    return EventType.MusicPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void addMusic(string musicPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ILyricRepo>();
                
                var musicPublishedDto = JsonSerializer.Deserialize<MusicPublishedDto>(musicPublishedMessage);

                try
                {
                    var plat = _mapper.Map<Music>(musicPublishedDto);
                    if(!repo.ExternalMusicExists(plat.ExternalID))
                    {
                        repo.CreateMusic(plat);
                        repo.SaveChanges();
                        Console.WriteLine("--> Music added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Music already exisits...");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Music to DB {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        MusicPublished,
        Undetermined
    }
}