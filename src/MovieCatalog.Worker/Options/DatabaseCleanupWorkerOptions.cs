namespace MovieCatalog.Worker.Options
{
    internal class DatabaseCleanupWorkerOptions
    {
        public bool IsEnabled { get; set; }
        public double ExecutionIntervalInMinutes { get; set; }
    }
}
