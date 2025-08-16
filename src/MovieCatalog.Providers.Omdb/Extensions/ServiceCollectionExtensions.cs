using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieCatalog.Providers.Omdb.Contracts.Client;
using MovieCatalog.Providers.Omdb.Contracts.IRepositories;
using MovieCatalog.Providers.Omdb.Contracts.Options;
using MovieCatalog.Providers.Omdb.Repositories;

namespace MovieCatalog.Providers.Omdb.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterOmdbProvider(this IServiceCollection serviceCollection, IConfiguration configuration, string optionsPath = "AppSettings:OmdbApiOptions")
        {
            serviceCollection.AddOptions<OmdbApiOptions>().Bind(configuration.GetSection(optionsPath));
            serviceCollection.AddHttpClient<OmdbApiClient>();

            serviceCollection.AddScoped<IOmdbMovieRepository, OmdbMovieRepository>();

            return serviceCollection;
        }
    }
}
