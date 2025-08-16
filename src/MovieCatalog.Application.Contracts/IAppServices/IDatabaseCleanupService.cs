namespace MovieCatalog.Application.Contracts.IAppServices
{
    public interface IDatabaseCleanupService
    {
        Task CleanupOldQueries();
    }
}
