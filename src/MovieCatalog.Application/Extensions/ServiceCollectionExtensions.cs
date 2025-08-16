using MovieCatalog.Application.Services.Extensions;
using MovieCatalog.Providers.Omdb.Extensions;

namespace MovieCatalog.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.RegisterOmdbProvider(configuration);
            serviceCollection.RegisterAppServices();

            return serviceCollection;
        }
    }
}
