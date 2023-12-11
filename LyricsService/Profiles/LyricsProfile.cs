using AutoMapper;
using LyricsService.Dtos;
using LyricsService.Models;
using MusicService;

namespace LyricsService.Profiles
{
    public class LyricsProfile : Profile
    {
        public LyricsProfile()
        {
            // Source -> Target
            CreateMap<Music, MusicreadDto>();
            CreateMap<LyricCreateDto, Lyric>();
            CreateMap<Lyric, LyricReadDto>();
            CreateMap<MusicPublishedDto, Music>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcMusicModel, Music>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.MusicId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Lyrics, opt => opt.Ignore());
        }
    }
}