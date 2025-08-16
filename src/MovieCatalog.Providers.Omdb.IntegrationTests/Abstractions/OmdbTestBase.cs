using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MovieCatalog.Providers.Omdb.Contracts.Client;
using MovieCatalog.Providers.Omdb.Contracts.Options;

namespace MovieCatalog.Providers.Omdb.IntegrationTests.Abstractions
{
    public class OmdbTestBase
    {
        private IConfiguration Configuration { get; }

        protected readonly OmdbApiClient OmdbApiClient;
        protected readonly OmdbImgApiClient OmdbImgApiClient;
        protected IOptions<OmdbApiOptions> OptionsConfig { get; set; }

        protected OmdbTestBase()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddUserSecrets<OmdbTestBase>()
                .Build();

            OptionsConfig = Options.Create(Configuration.GetSection("OmdbApi").Get<OmdbApiOptions>()!);

            OmdbApiClient = new OmdbApiClient(new HttpClient(), OptionsConfig);
            OmdbImgApiClient = new OmdbImgApiClient(new HttpClient(), OptionsConfig);
        }
    }
}
