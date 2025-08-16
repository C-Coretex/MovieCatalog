using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCatalog.Domain.Entities;

namespace MovieCatalog.Infrastructure.Configuration
{
    public class QueryHistoryEntityConfiguration: IEntityTypeConfiguration<QueryHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<QueryHistoryEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.QueryTitle)
                .IsRequired()
                .HasMaxLength(128);
            builder.Property(p => p.CreatedTimestamp).IsRequired();

            builder.HasIndex(p => p.CreatedTimestamp);
        }
    }
}
