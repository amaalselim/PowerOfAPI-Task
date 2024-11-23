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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Description)
                   .HasMaxLength(500);

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.StockQuantity)
                   .IsRequired();
        }
    }
}
