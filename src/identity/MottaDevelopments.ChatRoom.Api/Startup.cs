using System.Reflection;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MottaDevelopments.ChatRoom.Identity.Application.Autofac;
using MottaDevelopments.ChatRoom.Identity.Application.Registrations;
using MottaDevelopments.MicroServices.Application.Consul;
using MottaDevelopments.MicroServices.Application.JwtBearer;

namespace MottaDevelopments.ChatRoom.Identity.Api
{
    public class Startup
    {
        private const string ApplicationAssembly = "MottaDevelopments.ChatRoom.Identity.Application";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsulServices(Configuration)
                .AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .SetIsOriginAllowed((host) => true)
                            .AllowCredentials());
                })
                .AddJwtBearerConfiguration()
                .AddIdentityDbContext()
                .AddAutoMapper(config => config.AllowNullCollections = true, Assembly.Load(ApplicationAssembly))
                .AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder container) => container.AddAutoFacModules();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) 
                app.UseDeveloperExceptionPage();

            app.UseCors("CorsPolicy");
            
            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
