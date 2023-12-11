using AutoMapper;
using MusicService.Dto;
using MusicService.Dtos;
using MusicService.Models;

namespace MusicService.Profiles
{
    public class MusicsProfile : Profile
    {
        public MusicsProfile()
        {
            // Source -> Target
            CreateMap<Music, MusicReadDto>();
            CreateMap<MusicCreateDto, Music>();
            CreateMap<MusicReadDto, MusicPublishedDto>();
            CreateMap<Music, GrpcMusicModel>()
                .ForMember(dest => dest.MusicId, opt => opt.MapFrom(src =>src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>src.Name))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src =>src.Genre));
        }
    }
}