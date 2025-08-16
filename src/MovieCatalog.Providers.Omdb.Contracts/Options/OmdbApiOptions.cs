namespace MovieCatalog.Providers.Omdb.Contracts.Options
{
    public class OmdbApiOptions
    {
        public required string BaseUrl { get; init; }
        public required string ImgBaseUrl { get; init; }
        public required string ApiKey { get; init; }
    }
}
