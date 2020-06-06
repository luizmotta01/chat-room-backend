using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MottaDevelopments.ChatRoom.Identity.Application.Autofac;
using MottaDevelopments.ChatRoom.Identity.Application.Registrations;
using MottaDevelopments.MicroServices.Application.Consul;

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
            });
            
            services.AddIdentityDbContext()
                .AddAutoMapper(config => config.AllowNullCollections = true, Assembly.Load(ApplicationAssembly));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var identityUrl = Configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "chat-room-api";

            });

            services.AddControllers();
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
