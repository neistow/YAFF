using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using YAFF.Core.Configs;
using YAFF.Core.Entities.Identity;
using YAFF.Data;


namespace YAFF.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void BuildCors(this IServiceCollection services)
        {
            services.AddCors(o =>
            {
                o.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .WithMethods("GET", "POST", "PUT", "DELETE")
                        .AllowAnyHeader();
                });
            });
        }

        public static void AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtTokenConfig>(configuration.GetSection("JwtTokenConfig"));
            var jwtConfig = configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfig>();

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidateLifetime = true
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Jwt auth bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                var securityRequirement = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference {Id = "Bearer", Type = ReferenceType.SecurityScheme}
                };
                var openApiRequirement = new OpenApiSecurityRequirement {{securityRequirement, new List<string>()}};
                s.AddSecurityRequirement(openApiRequirement);
            });
        }

        public static void ConfigureAspNetIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<User>(o =>
                {
                    o.User.RequireUniqueEmail = true;
                    o.SignIn = new SignInOptions
                    {
                        RequireConfirmedEmail = true
                    };
                })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<ForumDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider)
                .AddSignInManager<SignInManager<User>>();
        }
    }
}