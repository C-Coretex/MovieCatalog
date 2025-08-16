using MovieCatalog.Domain.IRepositories;

namespace MovieCatalog.Infrastructure.Repositories
{
    internal class QueryHistoryRepository: IQueryHistoryRepository
    {
        public Task<bool> AddQueryEntity(string queryData)
        {
            throw new NotImplementedException();
        }
    }
}
