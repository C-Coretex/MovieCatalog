using MovieCatalog.Providers.Omdb.DTOs;
using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieCatalog.Providers.Omdb.Clients
{
    internal interface IOmdbApiClient
    {
        [Get("/")]
        Task<ApiResponse<OmdbShortResponseDto>> SearchByTitleAsync(
            [AliasAs(QueryNames.TitleAllName)] string title,
            [AliasAs(QueryNames.PageName)] int page,
            CancellationToken cancellationToken = default);

        [Get("/")]
        Task<ApiResponse<OmdbFullResponseDto>> GetByIdAsync(
            [AliasAs(QueryNames.IdFirstName)] string id,
            [AliasAs(QueryNames.PlotTypeName)] string plot,
            CancellationToken cancellationToken = default);


        private static class QueryNames
        {
            public const string IdFirstName = "i";
            public const string TitleAllName = "s";
            public const string PageName = "page";
            public const string PlotTypeName = "plot";
        }
    }

    internal class OmdbRefitSettings
    {
        public static RefitSettings Default { get; } = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                ReadCommentHandling = JsonCommentHandling.Skip,
            })
        };
    }
}
