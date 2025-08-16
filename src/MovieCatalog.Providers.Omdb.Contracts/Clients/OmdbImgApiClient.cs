using Microsoft.Extensions.Options;
using MovieCatalog.Providers.Omdb.Contracts.Clients;
using MovieCatalog.Providers.Omdb.Contracts.Options;

namespace MovieCatalog.Providers.Omdb.Contracts.Client
{
    public class OmdbImgApiClient : OmdbApiClientBase
    {
        public OmdbImgApiClient(HttpClient httpClient, IOptions<OmdbApiOptions> options): base(httpClient, options.Value.ImgBaseUrl, options.Value.ApiKey)
        {
            
        }
    }
}
