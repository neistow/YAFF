using System.Globalization;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using YAFF.Api.Extensions;
using YAFF.Business.Commands.Auth;
using YAFF.Business.Extensions;
using YAFF.Core.Entities.Identity;
using YAFF.Core.Mapper;
using YAFF.Core.Settings;
using YAFF.Data.Extensions;

namespace YAFF.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }

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
            services.ConfigureSwagger();

            services.ConfigureDbContext(Configuration);

            services.ConfigureAspNetIdentity();
            services.AddJwtBearerAuthentication(Configuration);

            services.BuildCors();

            services.AddControllers()
                .AddFluentValidation(o =>
                {
                    o.RegisterValidatorsFromAssembly(typeof(Startup).Assembly);
                    o.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    o.ValidatorOptions.LanguageManager.Enabled = false;
                });

            services.AddAutoMapper(typeof(Startup).Assembly, typeof(MapperConfig).Assembly);
            services.AddMediatR(typeof(Startup).Assembly, typeof(RegisterUserCommandHandler).Assembly);

            services.Configure<PhotoProcessorSettings>(Configuration.GetSection("PhotoProcessorSettings"));
            services.Configure<PhotoSettings>(Configuration.GetSection("PhotoSettings"));
            services.Configure<EmailSettings>(Configuration.GetSection("SmtpSettings"));

            services.AddPhotoStorage();
            services.AddImageProcessor();
            services.AddPhotoValidator();
            services.AddEmailSender();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseLoggingMiddleware();

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

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseLockoutMiddleware();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}