using Microsoft.EntityFrameworkCore;

namespace MovieCatalog.Infrastructure.Migrations.Factory
{
    public class MovieCatalogMigrationsDbContext: DbContext
    {
        public MovieCatalogMigrationsDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieCatalogDbContext).Assembly);
        }

        public static void ApplyMigrations(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder().UseSqlite(connectionString);
            var db = new MovieCatalogMigrationsDbContext(optionsBuilder.Options).Database;
            var pendingMigrations = db.GetPendingMigrations();

            if (pendingMigrations.Any())
                db.Migrate();
        }
    }
}
