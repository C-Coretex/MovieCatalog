using AutoBogus;
using AwesomeAssertions;
using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using MovieCatalog.Application.Contracts.DTOs;
using MovieCatalog.Application.Contracts.IAppServices;
using MovieCatalog.Application.Services.AppServices;
using MovieCatalog.Application.Services.UnitTests.Helpers;
using MovieCatalog.Domain.Entities;
using MovieCatalog.Domain.IRepositories;
using MovieCatalog.Providers.Omdb.Contracts.DTOs;
using MovieCatalog.Providers.Omdb.Contracts.IRepositories;

namespace MovieCatalog.Application.Services.UnitTests.AppServiceTests
{
    public class MovieCatalogAppServiceTests
    {
        private readonly Mock<ILogger<MovieCatalogAppService>> _loggerMock = new();
        private readonly Mock<IQueryHistoryRepository> _queryHistoryRepositoryMock = new();
        private List<QueryHistoryEntity> _queryHistoryEntities = [];
        private List<FullMovieOmdbDto> _omdbMovieEntities = [];
        private readonly Mock<IOmdbMovieRepository> _omdbMovieRepositoryMock = new();

        private readonly IMovieCatalogAppService _movieCatalogAppService;

        public MovieCatalogAppServiceTests()
        {
            SetupMocks();


            _movieCatalogAppService = new MovieCatalogAppService(
                _loggerMock.Object,
                _omdbMovieRepositoryMock.Object,
                _queryHistoryRepositoryMock.Object);
        }


        [Theory]
        [Trait("CleanupOldQueries", "Should delete all entries except latest provided amount and return amount of deleted entries")]
        [InlineData(null)]
        [InlineData(3)]
        [InlineData(25)]
        public async Task CleanupOldQueries_ShouldDeleteCorrectData(int? amountToTake)
        {
            List<QueryHistoryEntryDto> result;
            if (amountToTake.HasValue)
                result = await _movieCatalogAppService.GetLastQueryHistory(amountToTake.Value).ToListAsync();
            else
                result = await _movieCatalogAppService.GetLastQueryHistory().ToListAsync();

            amountToTake ??= 5; //default value
            result.Count.Should().Be(amountToTake.Value);

            result.Select(x => (x.Query, x.Timestamp)).Should().BeEquivalentTo(
                _queryHistoryEntities
                    .OrderByDescending(x => x.CreatedTimestamp)
                    .Take(amountToTake.Value)
                    .Select(x => (x.QueryTitle, x.CreatedTimestamp)).ToList(),
                options => options.WithStrictOrdering());
        }


        
        [Theory]
        [Trait("GetMoviesByTitle", "Should return all entries filtered by title. If there are no entries, should not fail")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetMoviesByTitle_ShouldReturnCorrectDataAndSaveHistory(bool existingTitle)
        {
            var originalQueryHistoryEntriesCount = _queryHistoryEntities.Count;

            var title = existingTitle
                ? _omdbMovieEntities.First().Title
                : new Faker().Random.Words(2);

            var result = await _movieCatalogAppService.GetMoviesByTitle(title, TestContext.Current.CancellationToken).ToListAsync();

            result.Should().NotBeEmpty();

            //Query history entry should be added
            _queryHistoryEntities.Count.Should().Be(originalQueryHistoryEntriesCount + 1);
            _queryHistoryEntities.OrderByDescending(x => x.CreatedTimestamp).Select(x => x.QueryTitle).First().Should().Be(title);

            //Result should contain entries by title with all required info
            if (existingTitle)
            {
                result.Should().OnlyContain(x => x.Entry!.Title.Contains(title));
                result.Count(x => !string.IsNullOrEmpty(x.Entry!.Title)).Should().BeGreaterThan(0);
                result.Count(x => !string.IsNullOrEmpty(x.Entry!.ImdbId)).Should().BeGreaterThan(0);
                result.Count(x => !string.IsNullOrEmpty(x.Entry!.Poster)).Should().BeGreaterThan(0);
                result.Count(x => !string.IsNullOrEmpty(x.Entry!.Type)).Should().BeGreaterThan(0);
                result.Count(x => !string.IsNullOrEmpty(x.Entry!.Year)).Should().BeGreaterThan(0);
            }
            else
                result.First().Successful.Should().BeFalse();
        }

        [Fact]
        [Trait("GetMoviesByTitle", "Should write to log and throw if repository exception")]
        public async Task GetMoviesByTitle_ShouldThrowAndLogOnRepositoryFailure()
        {
            _queryHistoryRepositoryMock.Setup(x => x.AddQueryEntry(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Some db exception"));

            var func = async () => await _movieCatalogAppService.GetMoviesByTitle("", TestContext.Current.CancellationToken).ToListAsync();

            await func.Should().ThrowAsync<Exception>();
            _loggerMock.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        }


        [Theory]
        [Trait("GetMovieDetailsById", "Should return all entries filtered by title. If there are no entries, should not fail")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetMovieDetailsById_ShouldReturnCorrectData(bool existingTitle)
        {
            var id = existingTitle
                ? _omdbMovieEntities.First().ImdbId
                : new Faker().Random.Words(2);

            var result = await _movieCatalogAppService.GetMovieDetailsById(id);

            result.Should().NotBeNull();
            if (existingTitle)
            {
                result.Successful.Should().BeTrue();
                result.Entry!.PosterUrl.Should().NotBeEmpty();
                result.Entry.Title.Should().NotBeEmpty();
                result.Entry.ImdbId.Should().NotBeEmpty();
                result.Entry.Type.Should().NotBeEmpty();
                result.Entry.Year.Should().NotBeEmpty();
                result.Entry.Rated.Should().NotBeEmpty();
                result.Entry.Released.Should().NotBeNull();
                result.Entry.Runtime.Should().NotBeEmpty();
                result.Entry.Genres.Should().NotBeEmpty();
                result.Entry.Director.Should().NotBeEmpty();
                result.Entry.Writer.Should().NotBeEmpty();
                result.Entry.Actors.Should().NotBeEmpty();
                result.Entry.Plot.Should().NotBeEmpty();
                result.Entry.Languages.Should().NotBeEmpty();
                result.Entry.Countries.Should().NotBeEmpty();
                result.Entry.Awards.Should().NotBeEmpty();
                result.Entry.Ratings.Should().NotBeEmpty();
                result.Entry.Metascore.Should().NotBeEmpty();
                result.Entry.ImdbRating.Should().NotBeNull();
                result.Entry.ImdbVotes.Should().NotBeNull();
                result.Entry.Dvd.Should().NotBeEmpty();
                result.Entry.BoxOffice.Should().NotBeEmpty();
                result.Entry.Production.Should().NotBeEmpty();
            }
            else
                result.Successful.Should().BeFalse();
        }


        private void SetupMocks()
        {
            var queryHistoryEntryFaker = new AutoFaker<QueryHistoryEntity>()
                .RuleFor(x => x.CreatedTimestamp, f => f.Date.Past(30, DateTime.UtcNow.AddMinutes(-1)));
            _queryHistoryEntities = queryHistoryEntryFaker.Generate(30);
            _queryHistoryRepositoryMock.EnrichQueryHistoryRepositoryMock(_queryHistoryEntities);

            var moviesCount = 50;

            //AutoFaker was failing on DateOnly? generation, so I just asked AI to write custom Faker
            var fullMovieOmdbDtoFaker = GetFullMovieOmdbDtoFaker(moviesCount);
            _omdbMovieEntities = fullMovieOmdbDtoFaker.Generate(moviesCount);

            _omdbMovieRepositoryMock.Setup(x => x.GetMoviesByTitle(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns<string, CancellationToken>((title, _) =>
                {
                    var data = _omdbMovieEntities.Where(x => x.Title.Contains(title)).ToList();
                    if (data.Count == 0)
                    {
                        IEnumerable<OmdbResult<ShortMovieOmdbDto>> entries = [OmdbResult<ShortMovieOmdbDto>.CreateFailed()];
                        return entries.ToAsyncEnumerable();
                    }

                    return data.Select(x => new OmdbResult<ShortMovieOmdbDto>(
                            true,
                            new ShortMovieOmdbDto(x.Title, x.Year, x.ImdbId, x.Type, x.PosterUrl)))
                        .ToAsyncEnumerable();
                });

            _omdbMovieRepositoryMock
                .Setup(x => x.GetMovieDetailsById(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns<string, CancellationToken>((id, _) =>
                {
                    var entity = _omdbMovieEntities.FirstOrDefault(x => x.ImdbId == id);
                    if (entity == null)
                        return Task.FromResult(new OmdbResult<FullMovieOmdbDto>(false, null));
                    return Task.FromResult(new OmdbResult<FullMovieOmdbDto>(true, entity));
                });
        }

        //faker generated by AI
        private Faker<FullMovieOmdbDto> GetFullMovieOmdbDtoFaker(int moviesCount)
        {
            var titles = new Faker().Random.WordsArray(moviesCount / 5 * 2);
            return new Faker<FullMovieOmdbDto>()
                .CustomInstantiator(f => new FullMovieOmdbDto(
                    Title: f.PickRandom(titles),
                    Year: f.Date.Past(50).Year.ToString(),
                    Rated: f.PickRandom("G", "PG", "PG-13", "R", "NC-17", "Not Rated"),
                    Released:DateOnly.FromDateTime(f.Date.Past(50, DateTime.Today)),
                    Runtime: $"{f.Random.Int(80, 180)} min",
                    Genres: f.PickRandom([
                        "Action", "Adventure", "Animation", "Biography", "Comedy", "Crime", "Documentary",
                        "Drama", "Family", "Fantasy", "History", "Horror", "Music", "Mystery", "Romance",
                        "Sci-Fi", "Sport", "Thriller", "War", "Western"
                    ], f.Random.Int(1, 4)).ToList(),
                    Director: f.Name.FullName(),
                    Writer: f.Name.FullName(),
                    Actors: Enumerable.Range(0, f.Random.Int(3, 8))
                        .Select(_ => f.Name.FullName())
                        .ToList(),
                    Plot: f.Lorem.Sentences(f.Random.Int(3, 8)),
                    Languages: f.PickRandom([
                        "English", "Spanish", "French", "German", "Italian", "Japanese", "Korean",
                        "Chinese", "Portuguese", "Russian", "Hindi", "Arabic"
                    ], f.Random.Int(1, 3)).ToList(),
                    Countries: f.PickRandom([
                        "USA", "UK", "Canada", "France", "Germany", "Italy", "Spain", "Japan",
                        "South Korea", "China", "Australia", "India", "Brazil", "Mexico"
                    ], f.Random.Int(1, 2)).ToList(),
                    Awards: f.Random.Bool(0.3f)
                        ? f.PickRandom(
                            "Winner of 1 Oscar",
                            "Nominated for 2 Oscars",
                            "Winner of 3 Golden Globes",
                            "Winner of Best Picture",
                            "N/A"
                        )
                        : "N/A",
                    PosterUrl: f.Internet.Url(),
                    Ratings: Enumerable.Range(0, f.Random.Int(1, 4))
                        .Select(_ => new RatingOmdbItemDto(
                            Source: f.PickRandom("Internet Movie Database", "Rotten Tomatoes", "Metacritic"),
                            Value: f.Random.Float(1.0f, 10.0f)
                        )).ToList(),
                    Metascore: f.Random.Bool(0.8f) ? f.Random.Int(20, 100).ToString() : "N/A",
                    ImdbRating: f.Random.Float(1.0f, 10.0f),
                    ImdbVotes: f.Random.Int(100, 2000000),
                    ImdbId: $"tt{f.Random.Int(1000000, 9999999)}",
                    Type: f.PickRandom("movie", "series", "episode"),
                    Dvd: f.Random.Bool(0.7f) ? f.Date.Past(30).ToString("dd MMM yyyy") : "N/A",
                    BoxOffice: f.Random.Bool(0.6f)
                        ? $"${f.Random.Int(1, 999):N0},{f.Random.Int(100, 999):N0},{f.Random.Int(100, 999):N0}"
                        : "N/A",
                    Production: f.Random.Bool(0.8f)
                        ? f.PickRandom("Warner Bros.", "Universal Pictures", "Paramount Pictures",
                                      "20th Century Fox", "Sony Pictures", "Disney", "MGM", "Lionsgate")
                        : "N/A",
                    Website: f.Random.Bool(0.6f) ? f.Internet.Url() : "N/A"
                ));
        }
    }
}
