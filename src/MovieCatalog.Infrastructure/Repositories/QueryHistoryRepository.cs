using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Domain.Entities;
using MovieCatalog.Domain.IRepositories;

namespace MovieCatalog.Infrastructure.Repositories
{
    //if there would be several repositories, we could introduce BaseRepository with basic method (GetAll with selector, GetById, Add/Update...)
    internal class QueryHistoryRepository: IQueryHistoryRepository
    {
        private readonly MovieCatalogDbContext _dbContext;
        private readonly DbSet<QueryHistoryEntity> _dbSet;
        public QueryHistoryRepository(MovieCatalogDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.QueryHistory;
        }

        public IAsyncEnumerable<TModel> GetLastEntries<TModel>(Expression<Func<QueryHistoryEntity, TModel>> selector, int take)
        {
            //don't need AsNoTracking, since we are applying selector
            return _dbSet
                .OrderByDescending(x => x.CreatedTimestamp)
                .Take(take)
                .Select(selector)
                .AsAsyncEnumerable();
        }

        public async Task<bool> AddQueryEntry(string queryData, CancellationToken cancellationToken = default)
        {
            var entity = new QueryHistoryEntity
            {
                QueryTitle = queryData,
                CreatedTimestamp = DateTime.UtcNow
            };
            _dbSet.Add(entity);

            var changes = await _dbContext.SaveChangesAsync(cancellationToken);
            return changes > 0;
        }

        public Task<int> DeleteOldEntries(int itemsToLeave, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.OrderByDescending(x => x.CreatedTimestamp).Skip(itemsToLeave);
            return query.ExecuteDeleteAsync(cancellationToken);
        }
    }
}
