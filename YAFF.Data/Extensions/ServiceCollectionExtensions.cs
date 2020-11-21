using Microsoft.Extensions.DependencyInjection;
using YAFF.Core.Interfaces.Data;
using YAFF.Core.Interfaces.Repositories;
using YAFF.Data.Repositories;

namespace YAFF.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddDbConnectionFactory(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        }
    }
}