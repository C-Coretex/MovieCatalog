using MovieCatalog.Application.Contracts.IAppServices;
using MovieCatalog.Domain.IRepositories;

namespace MovieCatalog.Application.Services.AppServices
{
    internal class DatabaseCleanupService(IQueryHistoryRepository queryHistoryRepository): IDatabaseCleanupService
    {
        public Task<int> CleanupOldQueries(int itemsToLeave = 5, CancellationToken cancellationToken = default)
        {
            return queryHistoryRepository.DeleteOldEntries(itemsToLeave, cancellationToken);
        }
    }
}
