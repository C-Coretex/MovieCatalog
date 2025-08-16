using MovieCatalog.Application.Services.Extensions;
using MovieCatalog.Infrastructure.Extensions;
using MovieCatalog.Providers.Omdb.Extensions;

namespace MovieCatalog.Worker.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection, IConfiguration configuration, string movieCatalogConnectionString)
        {
            serviceCollection.RegisterMovieCatalogDatabase(movieCatalogConnectionString);

            serviceCollection.RegisterOmdbProvider(configuration);
            serviceCollection.RegisterAppServices(); //if the project would be bigger, we would separate Application AppServices and Worker AppServices

            return serviceCollection;
        }
    }
}
