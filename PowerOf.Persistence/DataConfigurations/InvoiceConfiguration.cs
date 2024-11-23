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
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.InvoiceNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(i => i.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(i => i.TotalAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(i => i.Tax)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne(i => i.order)
                   .WithMany(o => o.Invoices)
                   .HasForeignKey(i => i.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
