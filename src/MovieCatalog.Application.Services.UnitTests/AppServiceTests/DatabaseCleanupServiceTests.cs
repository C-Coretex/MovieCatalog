using AutoBogus;
using AwesomeAssertions;
using Moq;
using MovieCatalog.Application.Contracts.IAppServices;
using MovieCatalog.Application.Services.AppServices;
using MovieCatalog.Application.Services.UnitTests.Helpers;
using MovieCatalog.Domain.Entities;
using MovieCatalog.Domain.IRepositories;

namespace MovieCatalog.Application.Services.UnitTests.AppServiceTests
{
    public class DatabaseCleanupServiceTests
    {
        private readonly Mock<IQueryHistoryRepository> _queryHistoryRepositoryMock = new();
        private List<QueryHistoryEntity> _entities = [];

        private readonly IDatabaseCleanupService _databaseCleanupService;

        public DatabaseCleanupServiceTests()
        {
            SetupMocks();
            _databaseCleanupService = new DatabaseCleanupService(_queryHistoryRepositoryMock.Object);
        }

        [Theory]
        [Trait("CleanupOldQueries", "Should delete all entries except latest provided amount and return amount of deleted entries")]
        [InlineData(null)]
        [InlineData(3)]
        [InlineData(25)]
        public async Task CleanupOldQueries_ShouldDeleteCorrectData(int? amountToLeave)
        {
            var originalEntitiesCount = _entities.Count;
            var orderedList = _entities.Select(x => x.CreatedTimestamp).Order().ToList();
            var minTimestamp = orderedList.First();
            var maxTimestamp = orderedList.Last();

            int value;
            if(amountToLeave.HasValue)
                value = await _databaseCleanupService.CleanupOldQueries(amountToLeave.Value);
            else
                value = await _databaseCleanupService.CleanupOldQueries();

            _entities.Count.Should().Be(originalEntitiesCount - value);

            amountToLeave ??= 5; //default value
            _entities.Count.Should().BeLessThanOrEqualTo(amountToLeave.Value);

            var resultTimestamps = _entities.Select(x => x.CreatedTimestamp).ToList();

            resultTimestamps.Should().NotContain(x => x == minTimestamp);
            resultTimestamps.Should().Contain(x => x == maxTimestamp);
        }

        private void SetupMocks()
        {
            var faker = new AutoFaker<QueryHistoryEntity>();
            _entities = faker.Generate(33);

            _queryHistoryRepositoryMock.EnrichQueryHistoryRepositoryMock(_entities);
        }
    }
}
