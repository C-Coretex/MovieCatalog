using System.Linq.Expressions;
using MovieCatalog.Domain.Entities;

namespace MovieCatalog.Domain.IRepositories
{
    public interface IQueryHistoryRepository
    {
        IAsyncEnumerable<TModel> GetLastEntries<TModel>(Expression<Func<QueryHistoryEntity, TModel>> selector, int take);
        
        /// <returns>true - if saved successfully.</returns>
        Task<bool> AddQueryEntry(string queryData, CancellationToken cancellationToken = default);

        /// <returns>Count of items deleted.</returns>
        Task<int> DeleteOldEntries(int itemsToLeave, CancellationToken cancellationToken = default);
    }
}
