using Microsoft.Extensions.Options;
using MovieCatalog.Application.Contracts.IAppServices;
using MovieCatalog.Worker.Options;

namespace MovieCatalog.Worker.Workers
{
    //if there would be multiple workers, we would create base worker
    internal class DatabaseCleanupWorker(
        ILogger<DatabaseCleanupWorker> logger, 
        IOptionsMonitor<DatabaseCleanupWorkerOptions> optionsMonitor, 
        IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var enabled = optionsMonitor.CurrentValue.IsEnabled;
                if (!enabled)
                {
                    // delay for appsettings change check
                    await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                    continue;
                }

                try
                {
                    await using var scope = serviceScopeFactory.CreateAsyncScope();
                    var service = scope.ServiceProvider.GetRequiredService<IDatabaseCleanupService>();
                    await service.CleanupOldQueries();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Worker Failed");
                }

                var delay = optionsMonitor.CurrentValue.ExecutionIntervalInMinutes;
                await Task.Delay(TimeSpan.FromMinutes(delay), cancellationToken);
            }
        }
    }
}
