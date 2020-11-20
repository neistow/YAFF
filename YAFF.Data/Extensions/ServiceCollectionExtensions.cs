using Microsoft.Extensions.DependencyInjection;
using YAFF.Core.Interfaces.Data;
using YAFF.Core.Interfaces.Repositories;
using YAFF.Data.Repositories;

namespace YAFF.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddDbConnectionFactory(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        }
    }
}