using Microsoft.EntityFrameworkCore;
using MovieCatalog.Domain.Entities;
using MovieCatalog.Infrastructure.Converters;

namespace MovieCatalog.Infrastructure
{
    public class MovieCatalogDbContext(DbContextOptions<MovieCatalogDbContext> options) : DbContext(options)
    {
        #region DbSets

        internal DbSet<QueryHistoryEntity> QueryHistory { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder
                .Properties<DateTime>()
                .HaveConversion<DateTimeUtcConverter>();
        }
    }
}
