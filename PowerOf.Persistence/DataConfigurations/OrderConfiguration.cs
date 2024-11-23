using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerOf.Core.Entities;

namespace PowerOf.Persistence.DataConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.UserId)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(o => o.OrderDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(o => o.TotalPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(o => o.Status)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasMany(o => o.Items)
                   .WithOne(i => i.Order)
                   .HasForeignKey(i => i.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
