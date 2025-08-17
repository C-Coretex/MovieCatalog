using MovieCatalog.Application.Services.Extensions;
using MovieCatalog.Infrastructure.Extensions;

namespace MovieCatalog.Worker.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection, IConfiguration configuration, string movieCatalogConnectionString)
        {
            serviceCollection.RegisterMovieCatalogDatabase(movieCatalogConnectionString);

            //if the project would be bigger, we would separate Application AppServices and Worker AppServices into different projects (at least .Contracts)
            serviceCollection.RegisterAppServicesWorker();

            return serviceCollection;
        }
    }
}
