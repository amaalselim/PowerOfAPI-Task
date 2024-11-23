using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerOf.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Persistence.DataConfigurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Quantity)
                   .IsRequired()
                   .HasDefaultValue(1);

            builder.Property(oi => oi.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.Items)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product)
                   .WithMany()
                   .HasForeignKey(oi => oi.ProductId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
