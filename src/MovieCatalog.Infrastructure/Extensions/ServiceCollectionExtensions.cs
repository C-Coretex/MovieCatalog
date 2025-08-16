using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieCatalog.Domain.IRepositories;
using MovieCatalog.Infrastructure.Repositories;

namespace MovieCatalog.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterMovieCatalogDatabase(this IServiceCollection services, string connectionString)
        {
            //not inmemory, because we have 2 projects that connect to the same db (.Application and .Worker)
            services.AddDbContext<MovieCatalogDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            services.AddScoped<IQueryHistoryRepository, QueryHistoryRepository>();

            return services;
        }
    }
}
