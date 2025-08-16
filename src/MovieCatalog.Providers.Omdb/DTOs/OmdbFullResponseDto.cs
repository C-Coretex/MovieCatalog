using System.Text.Json.Serialization;
using MovieCatalog.Providers.Omdb.Converters;

namespace MovieCatalog.Providers.Omdb.DTOs
{
    internal class OmdbFullResponseDto
    {
        [JsonConverter(typeof(Converters.BooleanConverter))]
        public bool Response { get; set; }

        public string? Title { get; set; }
        public string? Year { get; set; }
        public string? Rated { get; set; }

        [JsonConverter(typeof(Converters.DateOnlyConverter))]
        public DateOnly? Released { get; set; }
        public string? Runtime { get; set; }
        public string? Genre { get; set; }
        public string? Director { get; set; }
        public string? Writer { get; set; }
        public string? Actors { get; set; }
        public string? Plot { get; set; }
        public string? Language { get; set; }
        public string? Country { get; set; }
        public string? Awards { get; set; }
        public string? Poster { get; set; }
        public ICollection<RatingItemDto> Ratings { get; set; } = [];
        public string? Metascore { get; set; }

        [JsonConverter(typeof(Converters.FloatConverter))]
        public float? ImdbRating { get; set; }

        [JsonConverter(typeof(IntConverter))]
        public int? ImdbVotes { get; set; }
        public string? ImdbId { get; set; }
        public string? Type { get; set; }
        public string? Dvd { get; set; }
        public string? BoxOffice { get; set; }
        public string? Production { get; set; }
        public string? Website { get; set; }
    }

    internal class RatingItemDto
    {
        public required string Source { get; set; }
        [JsonConverter(typeof(Converters.RatingFloatConverter))]
        public required float Value { get; set; }
    }
}
