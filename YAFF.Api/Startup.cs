using System.Globalization;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YAFF.Api.Extensions;
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

            services.AddControllers();

            services.AddJwtBearerAuthentication(Configuration);

            services.AddAutoMapper(typeof(Startup).Assembly, typeof(MapperConfig).Assembly);
            services.AddMediatR(typeof(Startup).Assembly, typeof(CreateUserCommandHandler).Assembly);

            services.AddDbConnectionFactory();
            services.AddUnitOfWork();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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