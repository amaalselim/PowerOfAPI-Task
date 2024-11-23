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
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.Description)
                   .HasMaxLength(500);

            builder.Property(s => s.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(s => s.ServiceType)
                   .HasMaxLength(50);

            builder.Property(s => s.Rating)
                   .HasColumnType("float");

        }
    }
}
