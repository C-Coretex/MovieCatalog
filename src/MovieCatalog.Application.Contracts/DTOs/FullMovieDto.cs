namespace MovieCatalog.Application.Contracts.DTOs
{
    public record FullMovieDto(
        string Title,
        string Year,
        string Rated,
        DateOnly? Released,
        string Runtime,
        ICollection<string> Genres,
        string Director,
        string Writer,
        ICollection<string> Actors,
        string Plot,
        ICollection<string> Languages,
        ICollection<string> Countries,
        string Awards,
        string PosterUrl,
        ICollection<(string Source, int ValuePercent)> Ratings,
        string Metascore,
        float? ImdbRating,
        int? ImdbVotes,
        string ImdbId,
        string Type,
        string Dvd,
        string BoxOffice,
        string Production)
    {
    }
}
