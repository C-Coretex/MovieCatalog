namespace MovieCatalog.Domain.Entities
{
    public class QueryHistoryEntity
    {
        public Guid Id { get; init; } = Guid.CreateVersion7();

        public required string QueryTitle { get; set; }

        public DateTime CreatedTimestamp { get; set; }
    }
}
