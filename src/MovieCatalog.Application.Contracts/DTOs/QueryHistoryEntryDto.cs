using System.Linq.Expressions;
using MovieCatalog.Domain.Entities;

namespace MovieCatalog.Application.Contracts.DTOs
{
    public record QueryHistoryEntryDto(string Query, DateTime Timestamp)
    {
        public static Expression<Func<QueryHistoryEntity, QueryHistoryEntryDto>> Selector =>
            x => new QueryHistoryEntryDto(
                    x.QueryTitle,
                    x.CreatedTimestamp
                );
    }
}
