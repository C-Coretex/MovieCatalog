using MovieCatalog.Providers.Omdb.Contracts.DTOs;

namespace MovieCatalog.Providers.Omdb.Contracts.IRepositories
{
    public interface IOmdbMovieRepository
    {
        IAsyncEnumerable<OmdbResult<ShortMovieOmdbDto>> GetMoviesByTitle(string title, CancellationToken cancellationToken = default);
        Task<OmdbResult<FullMovieOmdbDto>> GetMovieDetailsById(string id, CancellationToken cancellationToken = default);
    }
}
