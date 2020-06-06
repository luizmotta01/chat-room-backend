using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MottaDevelopments.MicroServices.Application.Consul;

namespace MottaDevelopments.ChatRoom.Contacts.Api
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

            var secret = Environment.GetEnvironmentVariable("__JWT_SECRET__");

            var key = Encoding.ASCII.GetBytes(secret);

            services.AddAuthentication(scheme =>
                {
                    scheme.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    scheme.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(scheme =>
                {
                    scheme.RequireHttpsMetadata = false;
                    scheme.SaveToken = true;
                    scheme.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddControllers();
        }
        
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
