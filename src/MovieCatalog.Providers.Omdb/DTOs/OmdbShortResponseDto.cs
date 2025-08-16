using MovieCatalog.Providers.Omdb.Converters;
using System.Text.Json.Serialization;

namespace MovieCatalog.Providers.Omdb.DTOs
{
    internal class OmdbShortResponseDto
    {
        [JsonConverter(typeof(Converters.BooleanConverter))]
        public bool Response { get; set; }

        public ICollection<OmdbShortSearchItemDto> Search { get; set; } = [];
    }

    internal class OmdbShortSearchItemDto
    {
        public string? Title { get; set; }

        public string? Year { get; set; }
        public string? ImdbId { get; set; }
        public string? Type { get; set; }
        public string? Poster { get; set; }
    }
}
