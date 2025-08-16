namespace MovieCatalog.Domain.IRepositories
{
    public interface IQueryHistoryRepository
    {
        /// <returns>true - if saved successfully.</returns>
        Task<bool> AddQueryEntity(string queryData);
    }
}
