using Moq;
using MovieCatalog.Application.Contracts.DTOs;
using MovieCatalog.Domain.Entities;
using MovieCatalog.Domain.IRepositories;
using System.Linq.Expressions;

namespace MovieCatalog.Application.Services.UnitTests.Helpers
{
    internal static class MockHelpers
    {
        public static Mock<IQueryHistoryRepository> EnrichQueryHistoryRepositoryMock(this Mock<IQueryHistoryRepository> repository, List<QueryHistoryEntity> source)
        {
            var elementsDeleted = 0;
            repository.Setup(x => x.DeleteOldEntries(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Callback<int, CancellationToken>((itemsToDelete, _) =>
                {
                    var minTimestamp = source
                        .Select(x => x.CreatedTimestamp)
                        .OrderDescending()
                        .Skip(itemsToDelete)
                        .FirstOrDefault();

                    elementsDeleted = source.RemoveAll(x => x.CreatedTimestamp <= minTimestamp);
                })
                .Returns<int, CancellationToken>((_, _) => Task.FromResult(elementsDeleted));

            repository.Setup(x =>
                    x.GetLastEntries(It.IsAny<Expression<Func<QueryHistoryEntity, QueryHistoryEntryDto>>>(), It.IsAny<int>()))
                .Returns<Expression<Func<QueryHistoryEntity, QueryHistoryEntryDto>>, int>((selector, itemsToTake) =>
                    source.OrderByDescending(x => x.CreatedTimestamp).Select(selector.Compile()).Take(itemsToTake).ToAsyncEnumerable());

            repository.Setup(x => x.AddQueryEntry(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback<string, CancellationToken>((title, _) =>
                {
                    var entity = new QueryHistoryEntity()
                    {
                        QueryTitle = title,
                        CreatedTimestamp = DateTime.UtcNow
                    };
                    source.Add(entity);
                }).Returns<string, CancellationToken>((_, _) => Task.FromResult(true));

            return new Mock<IQueryHistoryRepository>();
        }
    }
}
