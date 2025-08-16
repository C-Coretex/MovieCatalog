namespace MovieCatalog.Providers.Omdb.Contracts.DTOs
{
    public record FullMovieOmdbDto(
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
        string Poster,
        ICollection<RatingOmdbItemDto> Ratings,
        string Metascore,
        float? ImdbRating,
        int? ImdbVotes,
        string ImdbId,
        string Type,
        string Dvd,
        string BoxOffice,
        string Production,
        string Website
        )
    {
    }
}
