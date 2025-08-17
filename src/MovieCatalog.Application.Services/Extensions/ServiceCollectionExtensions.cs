using Microsoft.Extensions.DependencyInjection;
using MovieCatalog.Application.Contracts.IAppServices;
using MovieCatalog.Application.Services.AppServices;

namespace MovieCatalog.Application.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAppServicesApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IMovieCatalogAppService, MovieCatalogAppService>();

            return serviceCollection;
        }

        public static IServiceCollection RegisterAppServicesWorker(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDatabaseCleanupService, DatabaseCleanupService>();

            return serviceCollection;
        }
    }
}
