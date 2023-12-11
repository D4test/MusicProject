using System.Threading.Tasks;
using MusicService.Dtos;

namespace MusicService.SyncDataServices.Http
{
    public interface ILyricDataClient
    {
        Task SendMusicToLyric(MusicReadDto plat); 
    }
}