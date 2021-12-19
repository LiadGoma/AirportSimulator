using AirportServer.Bl;
using AirportServer.Data;
using AirportServer.Hubs;
using AirportServer.repositories;
using DepartureSimulator;
using LandingSimulator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AirportServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddControllers();
            services.AddSingleton<IAirportRepository, AirportRepository>();
            services.AddSingleton<ILogic, Logic>();
            services.AddSingleton<ILandingSimulator, LandingSimulator.Simulator>();
            services.AddSingleton<IDepartureSimulator, DepartureSimulator.Simulator>();
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AirportContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Singleton);
            services.AddCors(opt =>
                   opt.AddPolicy("CorsPolicy", policy =>
                   {
                       policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:3000");
                   }));
           
          
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AirportContext ctx)
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");


            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AirportHub>("/movement");
            });
        }
    }
}
