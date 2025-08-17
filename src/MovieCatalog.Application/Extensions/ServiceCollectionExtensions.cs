using MovieCatalog.Application.Services.Extensions;
using MovieCatalog.Infrastructure.Extensions;
using MovieCatalog.Providers.Omdb.Extensions;

namespace MovieCatalog.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection, IConfiguration configuration, string movieCatalogConnectionString)
        {
            serviceCollection.RegisterMovieCatalogDatabase(movieCatalogConnectionString);

            serviceCollection.RegisterOmdbProvider(configuration);
            serviceCollection.RegisterAppServicesApplication();

            return serviceCollection;
        }
    }
}
