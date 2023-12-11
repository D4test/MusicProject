using System.Collections.Generic;
using LyricsService.Models;

namespace LyricsService.SyncDataServices.Grpc
{
    public interface IMusicDataClient
    {
        IEnumerable<Music> ReturnAllMusics();
    }
}