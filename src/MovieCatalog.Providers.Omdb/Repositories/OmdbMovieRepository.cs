using MovieCatalog.Providers.Omdb.Clients;
using MovieCatalog.Providers.Omdb.Constants;
using MovieCatalog.Providers.Omdb.Contracts.Clients;
using MovieCatalog.Providers.Omdb.Contracts.DTOs;
using MovieCatalog.Providers.Omdb.Contracts.IRepositories;
using MovieCatalog.Providers.Omdb.DTOs;
using MovieCatalog.Providers.Omdb.Helpers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieCatalog.Providers.Omdb.Repositories
{
    internal class OmdbMovieRepository(IOmdbApiClient client) : IOmdbMovieRepository
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
                var response = await client.SearchByTitleAsync(
                    title,
                    ++i,
                    cancellationToken);

                if (response.Error != null || response.Content?.Response != true)
                    break;
                var result = response.Content;

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
            var response = await client.GetByIdAsync(
                id,
                OmdbConstants.PlotTypes.Full,
                cancellationToken);

            if (response.Error != null || response.Content?.Response != true)
                return OmdbResult<FullMovieOmdbDto>.CreateFailed();
            var result = response.Content;

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
