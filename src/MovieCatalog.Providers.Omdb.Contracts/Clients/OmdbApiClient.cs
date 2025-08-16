using System.Web;
using Microsoft.Extensions.Options;
using MovieCatalog.Providers.Omdb.Contracts.Options;

namespace MovieCatalog.Providers.Omdb.Contracts.Clients
{
    public class OmdbApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string ApiKeyQueryParameterName = "apikey";

        public OmdbApiClient(HttpClient httpClient, IOptions<OmdbApiOptions> options)
        {
            _httpClient = httpClient;

            if (string.IsNullOrEmpty(options.Value.BaseUrl))
                throw new ArgumentException("Base URL must be provided.", nameof(options.Value.BaseUrl));
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);

            if (string.IsNullOrEmpty(options.Value.ApiKey))
                throw new ArgumentException("API key must be provided.", nameof(options.Value.ApiKey));
            _apiKey = options.Value.ApiKey;
        }

        private Uri EnrichQuery(string? path, IEnumerable<(string name, string value)> queryParams)
        {
            var uri = _httpClient.BaseAddress!;
            if (!string.IsNullOrEmpty(path))
                uri = new Uri(uri, path);

            var uriBuilder = new UriBuilder(uri);

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query[ApiKeyQueryParameterName] = _apiKey;

            foreach (var queryParameter in queryParams)
                query[queryParameter.name] = queryParameter.value;

            uriBuilder.Query = query.ToString()!;
            return uriBuilder.Uri;
        }

        public Task<HttpResponseMessage> GetAsync(CancellationToken cancellationToken = default, params (string name, string value)[] queryParams)
        {
            var uri = EnrichQuery(null, queryParams);
            return _httpClient.GetAsync(uri, cancellationToken);
        }
        public Task<HttpResponseMessage> GetAsync(string path, CancellationToken cancellationToken = default, params (string name, string value)[] queryParams)
        {
            var uri = EnrichQuery(path, queryParams);
            return _httpClient.GetAsync(uri, cancellationToken);
        }
    }
}
