using MovieCatalog.Application.Contracts.DTOs;

namespace MovieCatalog.Application.Contracts.IAppServices
{
    public interface IMovieCatalogAppService
    {
        IAsyncEnumerable<(bool Successful, ShortMovieDto? Entry)> GetMoviesByTitle(string title);
        Task<(bool Successful, FullMovieDto? Entry)> GetMovieDetailsById(string id);
    }
}
