using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using YAFF.Api.Hubs;
using YAFF.Api.Hubs.Common;
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
                        .WithOrigins("http://192.168.0.84:8080")
                        .WithMethods("GET", "POST", "PUT", "DELETE")
                        .AllowAnyHeader()
                        .AllowCredentials();
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
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        var accessToken = ctx.Request.Query["access_token"];
                        var path = ctx.HttpContext.Request.Path;
                        if (!string.IsNullOrWhiteSpace(accessToken) && path.StartsWithSegments("/chathub"))
                        {
                            ctx.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
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

        public static void ConfigureSignalR(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();
        }
    }
}