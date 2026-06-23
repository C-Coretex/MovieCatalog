using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MovieCatalog.Providers.Omdb.Clients;
using MovieCatalog.Providers.Omdb.Contracts.Clients;
using MovieCatalog.Providers.Omdb.Contracts.Clients.Handlers;
using MovieCatalog.Providers.Omdb.Contracts.IRepositories;
using MovieCatalog.Providers.Omdb.Contracts.Options;
using MovieCatalog.Providers.Omdb.Repositories;
using Refit;

namespace MovieCatalog.Providers.Omdb.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterOmdbProvider(this IServiceCollection serviceCollection, IConfiguration configuration, string optionsPath = "AppSettings:OmdbApiOptions")
        {
            serviceCollection.AddOptions<OmdbApiOptions>().Bind(configuration.GetSection(optionsPath));

            serviceCollection.AddTransient<OmdbApiKeyHandler>();

            serviceCollection.AddRefitClient<IOmdbApiClient>(OmdbRefitSettings.Default)
                .ConfigureHttpClient((sp, client) =>
                {
                    var options = sp.GetRequiredService<IOptions<OmdbApiOptions>>().Value;
                    if (string.IsNullOrEmpty(options.BaseUrl))
                        throw new ArgumentException("Base URL must be provided.", nameof(options.BaseUrl));
                    client.BaseAddress = new Uri(options.BaseUrl);
                }).AddHttpMessageHandler<OmdbApiKeyHandler>();

            serviceCollection.AddScoped<IOmdbMovieRepository, OmdbMovieRepository>();

            return serviceCollection;
        }
    }
}
