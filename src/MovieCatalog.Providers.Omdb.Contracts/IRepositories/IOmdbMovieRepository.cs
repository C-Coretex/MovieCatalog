using MovieCatalog.Providers.Omdb.Contracts.DTOs;

namespace MovieCatalog.Providers.Omdb.Contracts.IRepositories
{
    public interface IOmdbMovieRepository
    {
        IAsyncEnumerable<ShortMovieOmdbDto> GetMoviesByTitle(string title, CancellationToken cancellationToken = default);
        Task<FullMovieOmdbDto?> GetMovieDetailsById(string id, CancellationToken cancellationToken = default);
    }
}
