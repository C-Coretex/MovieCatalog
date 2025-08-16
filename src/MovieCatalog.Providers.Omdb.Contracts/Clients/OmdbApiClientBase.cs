using System.Threading;
using System.Web;

namespace MovieCatalog.Providers.Omdb.Contracts.Clients
{
    public class OmdbApiClientBase
    {
        protected readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string ApiKeyQueryParameterName = "apikey";

        public OmdbApiClientBase(HttpClient httpClient, string baseUrl, string apiKey)
        {
            _httpClient = httpClient;

            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentException("Base URL must be provided.", nameof(baseUrl));
            _httpClient.BaseAddress = new Uri(baseUrl);

            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("API key must be provided.", nameof(apiKey));
            _apiKey = apiKey;
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
