using System.Text.Json.Serialization;

namespace MovieCatalog.Providers.Omdb.DTOs
{
    internal class OmdbResponseDto
    {
        [JsonConverter(typeof(Converters.BooleanConverter))]
        public bool Response { get; set; }
    }
}
