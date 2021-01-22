using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YAFF.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ForumDbContext>(o => o.UseNpgsql(configuration["Data:ConnectionString"],
                b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
        }
    }
}