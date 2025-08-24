using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OutBoxPatternShipments
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("shipments");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                   .HasColumnName("id")
                   .IsRequired();

            builder.Property(s => s.OrderId)
                   .HasColumnName("orders_id")
                   .IsRequired();

            builder.Property(s => s.Status)
                   .HasColumnName("status")
                   .HasConversion<string>() // converte enum <-> string
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(s => s.UpdatedAt)
                   .HasColumnName("updated_at")
                   .IsRequired(false);

            builder.Property(s => s.CreatedAt)
                   .HasColumnName("created_at")
                   .IsRequired();
        }
    }
}
