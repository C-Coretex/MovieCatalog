using AwesomeAssertions;
using MovieCatalog.Providers.Omdb.Contracts.IRepositories;
using MovieCatalog.Providers.Omdb.IntegrationTests.Abstractions;
using MovieCatalog.Providers.Omdb.Repositories;


namespace MovieCatalog.Providers.Omdb.IntegrationTests.RepositoryTests
{
    public class OmdbMovieRepositoryTest: OmdbTestBase
    {
        private readonly IOmdbMovieRepository _omdbMovieRepository;

        public OmdbMovieRepositoryTest(): base()
        {
            _omdbMovieRepository = new OmdbMovieRepository(OmdbApiClient, OmdbImgApiClient);
        }

        [Theory]
        [Trait("GetMoviesByTitle", "Should return correct value")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetMovieByTitle_ShouldReturnMovieOrNull(bool existingMovie)
        {
            var title = existingMovie ? "Avatar" : "NON_EXISTING_MOVIEasdasdasd";
            var movies = await _omdbMovieRepository.GetMoviesByTitle(title).ToListAsync();

            movies.Should().NotBeNull();
            if (existingMovie)
            {
                movies.Should().NotBeEmpty();

                movies.Count.Should().BeGreaterThan(30); //make sure that pagination works

                movies.Count(movie => movie.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).Should().Be(movies.Count);
                movies.Count(movie => !string.IsNullOrEmpty(movie.ImdbId)).Should().Be(movies.Count);
                movies.Count(movie => !string.IsNullOrEmpty(movie.Year)).Should().BeGreaterThan(0);
                movies.Count(movie => !string.IsNullOrEmpty(movie.Poster)).Should().BeGreaterThan(0);
                movies.Count(movie => !string.IsNullOrEmpty(movie.Type)).Should().BeGreaterThan(0);
            }
            else
                movies.Should().BeEmpty();
        }

        [Theory]
        [Trait("GetMovieDetailsById", "Should return correct value")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetMovieDetails_ShouldReturnMovieDetails(bool existingMovie)
        {
            var id = existingMovie ? "tt0499549" : "tt9999999";
            var details = await _omdbMovieRepository.GetMovieDetailsById(id);

            if (existingMovie)
            {
                details.Should().NotBeNull();

                details.ImdbId.Should().Be(id);
                details.Title.Should().NotBeEmpty();
                details.Year.Should().NotBeEmpty();
                details.Actors.Should().NotBeEmpty();
                details.Director.Should().NotBeEmpty();
                details.Ratings.Count(x => x.Value is > 0 and < 100).Should().Be(details.Ratings.Count);
                details.Released.Should().BeAfter(DateOnly.MinValue);
                details.ImdbVotes.Should().BeGreaterThan(0);
                details.ImdbRating.Should().BeGreaterThan(0);
                details.Plot.Should().NotBeEmpty();
                details.Genres.Should().NotBeEmpty();
                details.Languages.Should().NotBeEmpty();
                details.Countries.Should().NotBeEmpty();
                details.Poster.Should().NotBeEmpty();
                details.Type.Should().NotBeEmpty();
                details.BoxOffice.Should().NotBeEmpty();
                details.Production.Should().NotBeEmpty();
                details.Dvd.Should().NotBeEmpty();
                details.Awards.Should().NotBeEmpty();
                details.Metascore.Should().NotBeEmpty();
                details.Runtime.Should().NotBeEmpty();
                details.Writer.Should().NotBeEmpty();
                details.Website.Should().NotBeEmpty();
                details.Poster.Should().StartWith("https://");
            }
            else
                details.Should().BeNull();
        }
    }
}
