using System;
using System.Globalization;
using System.IO;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using YAFF.Api.Extensions;
using YAFF.Business;
using YAFF.Business.Commands.Users;
using YAFF.Core.Mapper;
using YAFF.Data.Extensions;

namespace YAFF.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public IWebHostEnvironment HostEnvironment { get; set; }

        public Startup(IWebHostEnvironment environment)
        {
            HostEnvironment = environment;

            var builder = new ConfigurationBuilder()
                .SetBasePath(HostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{HostEnvironment.EnvironmentName}.json", false, true)
                .AddEnvironmentVariables();

            if (HostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.BuildCors();

            services.AddControllers()
                .AddFluentValidation(o =>
                {
                    o.RegisterValidatorsFromAssembly(typeof(Startup).Assembly);
                    o.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            services.ConfigureSwagger();

            services.AddJwtBearerAuthentication(Configuration);

            services.AddAutoMapper(typeof(Startup).Assembly, typeof(MapperConfig).Assembly);
            services.AddMediatR(typeof(Startup).Assembly, typeof(CreateUserCommandHandler).Assembly);

            services.AddDbConnectionFactory();
            services.AddUnitOfWork();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureLoggingMiddleware();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", "Api V1 Docs");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(env.WebRootPath),
                RequestPath = "/files"
            });

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture)
            });

            app.UseStatusCodePages();
            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/ping", async context => { await context.Response.WriteAsync("Pong"); });
                endpoints.MapControllers();
            });
        }
    }
}