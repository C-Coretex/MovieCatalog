using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MovieCatalog.Infrastructure.Migrations.Factory
{
    public class MovieCatalogMigrationsDbContextFactory : IDesignTimeDbContextFactory<MovieCatalogMigrationsDbContext>
    {
        public MovieCatalogMigrationsDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("MovieCatalogConnectionString");

            var optionsBuilder = new DbContextOptionsBuilder().UseSqlite(connectionString);
            return new MovieCatalogMigrationsDbContext(optionsBuilder.Options);
        }
    }
}
