using MusicService.Dtos;

namespace MusicService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewMusic(MusicPublishedDto musicPublishedDto);
    }
}