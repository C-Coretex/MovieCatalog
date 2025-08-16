using MovieCatalog.Application.Contracts.IAppServices;

namespace MovieCatalog.Application.Services.AppServices
{
    internal class DatabaseCleanupService: IDatabaseCleanupService
    {
        public Task CleanupOldQueries()
        {
            throw new NotImplementedException();
        }
    }
}
