using MovieCatalog.Application.Contracts.DTOs;
using MovieCatalog.Application.Contracts.IAppServices;
using MovieCatalog.Providers.Omdb.Contracts.IRepositories;

namespace MovieCatalog.Application.Services.AppServices
{
    internal class MovieCatalogAppService(IOmdbMovieRepository omdbMovieRepository): IMovieCatalogAppService
    {
        public IAsyncEnumerable<(bool Successful, ShortMovieDto? Entry)> GetMoviesByTitle(string title)
        {
            return omdbMovieRepository.GetMoviesByTitle(title)
                .Select(movie => (
                    movie.Success,
                    movie is { Success: true, Data: not null } ? new ShortMovieDto(
                        movie.Data.Title,
                        movie.Data.Year,
                        movie.Data.ImdbId,
                        movie.Data.Type,
                        movie.Data.Poster) : null
                    ));
        }
    }
}
