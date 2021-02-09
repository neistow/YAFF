using Microsoft.Extensions.DependencyInjection;
using YAFF.Business.Common;
using YAFF.Core.Interfaces;

namespace YAFF.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPhotoStorage(this IServiceCollection services)
        {
            services.AddScoped<IPhotoStorage, FileSystemPhotoStorage>();
        }

        public static void AddImageProcessor(this IServiceCollection services)
        {
            services.AddScoped<IImageProcessor, ImageProcessor>();
        }

        public static void AddPhotoValidator(this IServiceCollection services)
        {
            services.AddScoped<IPhotoValidator, PhotoValidator>();
        }
    }
}