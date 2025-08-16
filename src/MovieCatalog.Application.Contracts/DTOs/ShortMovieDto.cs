namespace MovieCatalog.Application.Contracts.DTOs
{
    public record ShortMovieDto(
        string Title,
        string Year,
        string ImdbId,
        string Type,
        string Poster)
    {
    }
}
