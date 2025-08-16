using Microsoft.Extensions.Logging;
using MovieCatalog.Application.Contracts.DTOs;
using MovieCatalog.Application.Contracts.IAppServices;
using MovieCatalog.Domain.IRepositories;
using MovieCatalog.Providers.Omdb.Contracts.IRepositories;

namespace MovieCatalog.Application.Services.AppServices
{
    internal class MovieCatalogAppService(ILogger<MovieCatalogAppService> logger, IOmdbMovieRepository omdbMovieRepository, IQueryHistoryRepository queryHistoryRepository) : IMovieCatalogAppService
    {
        public IAsyncEnumerable<QueryHistoryEntryDto> GetLastQueryHistory(int amount = 5)
            => queryHistoryRepository.GetLastEntries(QueryHistoryEntryDto.Selector, amount);

        public async IAsyncEnumerable<(bool Successful, ShortMovieDto? Entry)> GetMoviesByTitle(string title)
        {
            //in a bigger project we would have exception handler for all Blazor service calls to not repeat try-catch every time
            try
            {
                await queryHistoryRepository.AddQueryEntry(title);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception on query entity save");
                throw;
            }

            var entries = omdbMovieRepository.GetMoviesByTitle(title)
                .Select(movie => (
                    movie.Success,
                    movie is { Success: true, Data: not null } ? new ShortMovieDto(
                        movie.Data.Title,
                        movie.Data.Year,
                        movie.Data.ImdbId,
                        movie.Data.Type,
                        movie.Data.Poster) : null
                    ));

            await foreach (var item in entries)
                yield return item;
        }

        public async Task<(bool Successful, FullMovieDto? Entry)> GetMovieDetailsById(string id)
        {
            var response = await omdbMovieRepository.GetMovieDetailsById(id);
            if (!response.Success)
                return (false, null);

            var value = response.Data!;
            return (true, new FullMovieDto(
                Title: value.Title,
                Year: value.Year,
                Rated: value.Rated,
                Released: value.Released,
                Runtime: value.Runtime,
                Genres: value.Genres,
                Director: value.Director,
                Writer: value.Writer,
                Actors: value.Actors,
                Plot: value.Plot,
                Languages: value.Languages,
                Countries: value.Countries,
                Awards: value.Awards,
                PosterUrl: value.PosterUrl,
                Ratings: value.Ratings.Select(r => (r.Source, (int)(r.Value * 100))).ToList(),
                Metascore: value.Metascore,
                ImdbRating: value.ImdbRating,
                ImdbVotes: value.ImdbVotes,
                ImdbId: value.ImdbId,
                Type: value.Type,
                Dvd: value.Dvd,
                BoxOffice: value.BoxOffice,
                Production: value.Production
            ));
        }
    }
}
