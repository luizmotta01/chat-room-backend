using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MottaDevelopments.MicroServices.Application.JwtBearer
{
    public static class Extensions
    {
        public static IServiceCollection AddJwtBearerConfiguration(this IServiceCollection services)
        {
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

            return services;
        }
    }
}