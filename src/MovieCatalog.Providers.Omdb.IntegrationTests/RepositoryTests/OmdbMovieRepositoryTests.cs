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

                movies.Count(movie => movie.Data!.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).Should().Be(movies.Count);
                movies.Count(movie => !string.IsNullOrEmpty(movie.Data!.ImdbId)).Should().Be(movies.Count);
                movies.Count(movie => !string.IsNullOrEmpty(movie.Data!.Year)).Should().BeGreaterThan(0);
                movies.Count(movie => !string.IsNullOrEmpty(movie.Data!.Poster)).Should().BeGreaterThan(0);
                movies.Count(movie => !string.IsNullOrEmpty(movie.Data!.Type)).Should().BeGreaterThan(0);
            }
            else
                movies.Select(x => x.Success).Should().BeEquivalentTo([false]);
        }

        [Theory]
        [Trait("GetMovieDetailsById", "Should return correct value")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetMovieDetails_ShouldReturnMovieDetails(bool existingMovie)
        {
            var id = existingMovie ? "tt0499549" : "tt9999999";
            var details = await _omdbMovieRepository.GetMovieDetailsById(id);

            details.Should().NotBeNull();
            if (existingMovie)
            {
                details.Success.Should().Be(true);
                details.Data!.ImdbId.Should().Be(id);
                details.Data!.Title.Should().NotBeEmpty();
                details.Data!.Year.Should().NotBeEmpty();
                details.Data!.Actors.Should().NotBeEmpty();
                details.Data!.Director.Should().NotBeEmpty();
                details.Data!.Ratings.Count(x => x.Value is > 0 and < 100).Should().Be(details.Data!.Ratings.Count);
                details.Data!.Released.Should().BeAfter(DateOnly.MinValue);
                details.Data!.ImdbVotes.Should().BeGreaterThan(0);
                details.Data!.ImdbRating.Should().BeGreaterThan(0);
                details.Data!.Plot.Should().NotBeEmpty();
                details.Data!.Genres.Should().NotBeEmpty();
                details.Data!.Languages.Should().NotBeEmpty();
                details.Data!.Countries.Should().NotBeEmpty();
                details.Data!.PosterUrl.Should().NotBeEmpty();
                details.Data!.Type.Should().NotBeEmpty();
                details.Data!.BoxOffice.Should().NotBeEmpty();
                details.Data!.Production.Should().NotBeEmpty();
                details.Data!.Dvd.Should().NotBeEmpty();
                details.Data!.Awards.Should().NotBeEmpty();
                details.Data!.Metascore.Should().NotBeEmpty();
                details.Data!.Runtime.Should().NotBeEmpty();
                details.Data!.Writer.Should().NotBeEmpty();
                details.Data!.Website.Should().NotBeEmpty();
                details.Data!.PosterUrl.Should().StartWith("https://");
            }
            else
                details.Success.Should().Be(false);
        }
    }
}
