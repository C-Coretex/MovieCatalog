using MovieCatalog.Providers.Omdb.Constants;
using MovieCatalog.Providers.Omdb.Contracts.Client;
using MovieCatalog.Providers.Omdb.Contracts.DTOs;
using MovieCatalog.Providers.Omdb.Contracts.IRepositories;
using MovieCatalog.Providers.Omdb.DTOs;
using MovieCatalog.Providers.Omdb.Helpers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MovieCatalog.Providers.Omdb.Repositories
{
    internal class OmdbMovieRepository(OmdbApiClient client, OmdbImgApiClient imgClient) : IOmdbMovieRepository
    {
        private const int PageSize = 10;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };

    public async IAsyncEnumerable<OmdbResult<ShortMovieOmdbDto>> GetMoviesByTitle(string title, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var i = 0;
            var totalData = -1;
            bool isSuccess;
            do
            {
                //in future if the repository would support more filters/options, we could introduce request builder e.g. request.AddFilter(new TitleFilter(title));
                var data = await client.GetAsync(
                    cancellationToken,
                    (OmdbConstants.QueryNames.TitleAllName, title),
                    (OmdbConstants.QueryNames.PageName, (++i).ToString()));

                var json = await data.Content.ReadAsStringAsync(cancellationToken);

                try
                {
                    var response = JsonSerializer.Deserialize<OmdbResponseDto>(json, _jsonOptions);
                    if (response?.Response != true)
                        break;
                }
                catch (JsonException)
                {
                    // Using this, because sometimes with incorrect Id passed, API returns error with incorrect Json (there are '"' inside of string value)
                    // JsonDeserializer throws the exception and the easiest way to handle it is to catch it.
                    break;
                }

                var result = JsonSerializer.Deserialize<OmdbShortResponseDto>(json, _jsonOptions);
                isSuccess = result?.Response == true;
                if (!isSuccess) 
                    break;

                if(result?.TotalResults.HasValue == true)
                    totalData = result.TotalResults.Value;

                foreach (var item in result!.Search)
                    yield return OmdbResult<ShortMovieOmdbDto>.CreateSuccessful(
                        new ShortMovieOmdbDto(
                            item.Title.FormatNullableString(),
                            item.Year.FormatNullableString(),
                            item.ImdbId.FormatNullableString(),
                            item.Type.FormatNullableString(),
                            item.Poster.FormatNullableString()));
            } while (isSuccess);

            //we didn't get any data, or we didn't reach to the end
            if(totalData == -1 || PageSize * i < totalData)
                yield return OmdbResult<ShortMovieOmdbDto>.CreateFailed();
        }

        public async Task<OmdbResult<FullMovieOmdbDto>> GetMovieDetailsById(string id, CancellationToken cancellationToken = default)
        {
            var data = await client.GetAsync(
                cancellationToken,
                (OmdbConstants.QueryNames.IdFirstName, id),
                (OmdbConstants.QueryNames.PlotTypeName, OmdbConstants.PlotTypes.Full));

            var json = await data.Content.ReadAsStringAsync(cancellationToken);

            try
            {
                var response = JsonSerializer.Deserialize<OmdbResponseDto>(json, _jsonOptions);
                if (response?.Response != true)
                    return OmdbResult<FullMovieOmdbDto>.CreateFailed();
            }
            catch (JsonException)
            {
                // Using this, because sometimes with incorrect Id passed, API returns error with incorrect Json (there are '"' inside of string value)
                // JsonDeserializer throws the exception and the easiest way to handle it is to catch it.
                return OmdbResult<FullMovieOmdbDto>.CreateFailed();
            }

            var result = JsonSerializer.Deserialize<OmdbFullResponseDto>(json, _jsonOptions);
            if (result?.Response != true)
                return OmdbResult<FullMovieOmdbDto>.CreateFailed();

            return OmdbResult<FullMovieOmdbDto>.CreateSuccessful(new FullMovieOmdbDto(
                result.Title.FormatNullableString(),
                result.Year.FormatNullableString(),
                result.Rated.FormatNullableString(),
                result.Released ?? DateOnly.MinValue,
                result.Runtime.FormatNullableString(),
                result.Genre?.Split(", ") ?? [],
                result.Director.FormatNullableString(),
                result.Writer.FormatNullableString(),
                result.Actors?.Split(", ") ?? [],
                result.Plot.FormatNullableString(),
                result.Language?.Split(", ") ?? [],
                result.Country?.Split(", ") ?? [],
                result.Awards.FormatNullableString(),
                result.Poster.FormatNullableString(),
                result.Ratings.Select(r => new RatingOmdbItemDto(r.Source, r.Value)).ToList(),
                result.Metascore.FormatNullableString(),
                result.ImdbRating ?? 0,
                result.ImdbVotes ?? 0,
                result.ImdbId.FormatNullableString(),
                result.Type.FormatNullableString(),
                result.Dvd.FormatNullableString(),
                result.BoxOffice.FormatNullableString(),
                result.Production.FormatNullableString(),
                result.Website.FormatNullableString()));
        }
    }
}
