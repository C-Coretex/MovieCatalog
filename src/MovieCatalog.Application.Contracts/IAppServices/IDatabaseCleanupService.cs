namespace MovieCatalog.Application.Contracts.IAppServices
{
    public interface IDatabaseCleanupService
    {
        Task<int> CleanupOldQueries(int itemsToLeave = 5, CancellationToken cancellationToken = default);
    }
}
