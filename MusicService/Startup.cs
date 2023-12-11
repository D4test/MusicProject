using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MusicService.AsyncDataServices;
using MusicService.Data;
using MusicService.SyncDataServices.Grpc;
using MusicService.SyncDataServices.Http;

namespace MusicService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }
        
        
        
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
              Console.WriteLine("--> Using SqlServer Db");
              services.AddDbContext<AppDbContext>(opt =>
                  opt.UseSqlServer(Configuration.GetConnectionString("MusicsConn")));
            }
            else
            {
                Console.WriteLine("--> Using InMem Db");
                services.AddDbContext<AppDbContext>(opt =>
                     opt.UseInMemoryDatabase("InMem"));
            }

            services.AddScoped<IMusicRepo, MusicRepo>();

            services.AddHttpClient<ILyricDataClient, HttpLyricDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddGrpc();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MusicService", Version = "v1" });
            });

            Console.WriteLine($"--> LyricService Endpoint {Configuration["LyricService"]}");

        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MusicService v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcMusicService>();

                endpoints.MapGet("/protos/musics.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/musics.proto"));
                });
            });


            PrepDb.PrepPopulation(app, env.IsProduction());
            
        }
    }
}
