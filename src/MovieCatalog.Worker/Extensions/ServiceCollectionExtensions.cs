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

            //if the project would be bigger, we would separate Application AppServices and Worker AppServices
            //then we wouldn't need to register RegisterOmdbProvider, since we are not using it in any Worker AppService
            serviceCollection.RegisterOmdbProvider(configuration);
            serviceCollection.RegisterAppServices();

            return serviceCollection;
        }
    }
}
