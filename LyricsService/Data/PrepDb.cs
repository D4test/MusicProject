using System;
using System.Collections.Generic;
using LyricsService.Models;
using LyricsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LyricsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IMusicDataClient>();

                var musics = grpcClient.ReturnAllMusics();

                SeedData(serviceScope.ServiceProvider.GetService<ILyricRepo>(), musics);
            }
        }
        
        private static void SeedData(ILyricRepo repo, IEnumerable<Music> musics)
        {
            Console.WriteLine("Seeding new musics...");
            if (null == musics) 
            {
                return;
            }

            foreach (var plat in musics)
            {
                if(!repo.ExternalMusicExists(plat.ExternalID))
                {
                    repo.CreateMusic(plat);
                }
                repo.SaveChanges();
            }
        }
    }
}