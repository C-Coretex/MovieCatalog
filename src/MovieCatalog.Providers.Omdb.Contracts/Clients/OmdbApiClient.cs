using Microsoft.Extensions.Options;
using MovieCatalog.Providers.Omdb.Contracts.Clients;
using MovieCatalog.Providers.Omdb.Contracts.Options;

namespace MovieCatalog.Providers.Omdb.Contracts.Client
{
    public class OmdbApiClient: OmdbApiClientBase
    {
        public OmdbApiClient(HttpClient httpClient, IOptions<OmdbApiOptions> options): base(httpClient, options.Value.BaseUrl, options.Value.ApiKey)
        {
            
        }
    }
}
