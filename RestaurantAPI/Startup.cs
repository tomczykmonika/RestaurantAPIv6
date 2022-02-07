using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RestaurantAPI.Services;

namespace RestaurantAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<IWeatherForecastService, WeatherForecastService>();
            //Bêdziemy mieæ pewnoœæ ¿e dana zale¿noœæ bêd¹ wywo³ana tylko raz podczas ca³ego czasu u¿ywania aplikacji (od uruchomienia do zamkniêcia).

            //services.AddScoped<IWeatherForecastService, WeatherForecastService>();
            //Ka¿dy obiekt jest tworzony na nowo przy ka¿dym zapytaniu klienta. Na 1 zapytanie bêdziemy mieæ 1 instacjê danego serwisu.

            //services.AddTransient<IWeatherForecastService, WeatherForecastService>();
            //Obiekty bêd¹ utworzone za ka¿dym razem, kiedy odwo³ujemy siê do nich przez konstruktor.

            services.AddControllers();
            services.AddDbContext<RestaurantDbContext>();
            services.AddScoped<RestaurantSeeder>();

            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<IRestaurantService, RestaurantService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestaurantSeeder seeder)
        {
            seeder.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
