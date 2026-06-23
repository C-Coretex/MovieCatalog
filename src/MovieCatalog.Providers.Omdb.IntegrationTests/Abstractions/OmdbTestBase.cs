using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MovieCatalog.Providers.Omdb.Clients;
using MovieCatalog.Providers.Omdb.Contracts.Clients;
using MovieCatalog.Providers.Omdb.Contracts.Clients.Handlers;
using MovieCatalog.Providers.Omdb.Contracts.Options;
using Refit;

namespace MovieCatalog.Providers.Omdb.IntegrationTests.Abstractions
{
    public class OmdbTestBase
    {
        private IConfiguration Configuration { get; }

        internal readonly IOmdbApiClient OmdbApiClient;
        protected IOptions<OmdbApiOptions> OptionsConfig { get; set; }

        protected OmdbTestBase()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddUserSecrets<OmdbTestBase>()
                .Build();

            OptionsConfig = Options.Create(Configuration.GetSection("OmdbApi").Get<OmdbApiOptions>()!);

            var apiKeyHandler = new OmdbApiKeyHandler(OptionsConfig) { InnerHandler = new HttpClientHandler() };
            var httpClient = new HttpClient(apiKeyHandler) { BaseAddress = new Uri(OptionsConfig.Value.BaseUrl) };
            OmdbApiClient = RestService.For<IOmdbApiClient>(httpClient, OmdbRefitSettings.Default);
        }
    }
}
