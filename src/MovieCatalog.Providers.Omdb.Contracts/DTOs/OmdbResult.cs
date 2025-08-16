namespace MovieCatalog.Providers.Omdb.Contracts.DTOs
{
    public record OmdbResult<TData>(
        bool Success,
        TData? Data)
    {
        public static OmdbResult<TData> CreateSuccessful(TData data) => new OmdbResult<TData>(true, data);
        public static OmdbResult<TData> CreateFailed() => new OmdbResult<TData>(false, default);
    }
}
