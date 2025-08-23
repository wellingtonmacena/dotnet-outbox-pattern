using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutboxPatternOrders;

namespace OutboxPattern.Infrastructure.Configurations
{
    public class OrderOutboxConfiguration : IEntityTypeConfiguration<OrdersOutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OrdersOutboxMessage> builder)
        {
            // Tabela
            builder.ToTable("orders_outbox");

            // Chave primária GUID
            builder.HasKey(o => o.Id);

            // Gerar GUID no banco automaticamente
            builder.Property(o => o.Id)
                   .HasColumnName("id");

            builder.Property(o => o.OrderId)
                  .HasColumnName("order_id");

            // Colunas restantes
            builder.Property(o => o.Type)
                   .HasColumnName("type")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(o => o.Content)
                   .HasColumnName("content")
                   .IsRequired();


            builder.Property(o => o.ProcessedOnUtc)
                   .HasColumnName("processed_on_utc")
            .IsRequired(false);
            builder.Property(o => o.UpdatedAt)
              .HasColumnName("updated_at")
              .IsRequired(false); 
             

            builder.Property(o => o.CreatedAt)
                 .HasColumnName("created_at")
                 .IsRequired();

            builder.Property(o => o.Error)
                   .HasColumnName("error")
                    .IsRequired(false);
        }
    }
}
