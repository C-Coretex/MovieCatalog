using MovieCatalog.Infrastructure.Migrations.Factory;
using MovieCatalog.Worker;
using MovieCatalog.Worker.Extensions;
using MovieCatalog.Worker.Options;
using MovieCatalog.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);

var movieCatalogConnectionString = builder.Configuration.GetConnectionString("MovieCatalogConnectionString")
                                   ?? throw new ArgumentException("'MovieCatalogConnectionString' is not configured.");

builder.Services.RegisterServices(builder.Configuration, movieCatalogConnectionString);
AddWorkers(builder.Services);
AddOptions(builder.Services, builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    MovieCatalogMigrationsDbContext.ApplyMigrations(movieCatalogConnectionString);
}

var host = builder.Build();

host.Run();

static void AddWorkers(IServiceCollection services)
{
    services.AddHostedService<DatabaseCleanupWorker>();
}
static void AddOptions(IServiceCollection services, ConfigurationManager configuration)
{
    services.AddOptions<DatabaseCleanupWorkerOptions>().Bind(configuration.GetSection("AppSettings:DatabaseCleanupWorkerOptions"));
}