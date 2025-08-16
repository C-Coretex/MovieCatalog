using MovieCatalog.Worker;
using MovieCatalog.Worker.Extensions;
using MovieCatalog.Worker.Options;
using MovieCatalog.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.RegisterServices(builder.Configuration);
AddWorkers(builder.Services);
AddOptions(builder.Services, builder.Configuration);

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