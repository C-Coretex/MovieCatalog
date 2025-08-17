using MovieCatalog.Application.Contracts.DTOs;

namespace MovieCatalog.Application.Contracts.IAppServices
{
    public interface IMovieCatalogAppService
    {
        IAsyncEnumerable<QueryHistoryEntryDto> GetLastQueryHistory(int amount = 5);
        IAsyncEnumerable<(bool Successful, ShortMovieDto? Entry)> GetMoviesByTitle(string title,
            CancellationToken cancellationToken);
        Task<(bool Successful, FullMovieDto? Entry)> GetMovieDetailsById(string id);
    }
}
