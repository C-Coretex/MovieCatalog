namespace MovieCatalog.Providers.Omdb.Contracts.DTOs
{
    public record ShortMovieOmdbDto(
        string Title,
        string Year,
        string ImdbId,
        string Type,
        string Poster)
    {}
}
