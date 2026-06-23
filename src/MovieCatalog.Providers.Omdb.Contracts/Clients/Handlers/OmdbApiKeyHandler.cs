using Microsoft.Extensions.Options;
using MovieCatalog.Providers.Omdb.Contracts.Options;
using System.Web;

namespace MovieCatalog.Providers.Omdb.Contracts.Clients.Handlers
{
    public class OmdbApiKeyHandler: DelegatingHandler
    {
        private readonly string _apiKey;
        private const string ApiKeyQueryParameterName = "apikey";

        public OmdbApiKeyHandler(IOptions<OmdbApiOptions> options)
        {
            if (string.IsNullOrEmpty(options.Value.ApiKey))
                throw new ArgumentException("API key must be provided.", nameof(options));
            _apiKey = options.Value.ApiKey;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder(request.RequestUri!);

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[ApiKeyQueryParameterName] = _apiKey;

            uriBuilder.Query = query.ToString()!;
            request.RequestUri = uriBuilder.Uri;

            return base.SendAsync(request, cancellationToken);
        }
    }
}
