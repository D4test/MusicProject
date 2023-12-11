using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using MusicService.Data;

namespace MusicService.SyncDataServices.Grpc
{
    public class GrpcMusicService : GrpcMusic.GrpcMusicBase
    {
        private readonly IMusicRepo _repository;
        private readonly IMapper _mapper;

        public GrpcMusicService(IMusicRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<MusicResponse> GetAllMusics(GetAllRequest request, ServerCallContext context)
        {
            var response = new MusicResponse();
            var musics = _repository.GetAllMusics();

            foreach(var plat in musics)
            {
                response.Music.Add(_mapper.Map<GrpcMusicModel>(plat));
            }

            return Task.FromResult(response);
        }
    }
}