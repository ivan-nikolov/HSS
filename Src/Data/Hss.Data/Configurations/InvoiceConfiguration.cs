namespace Hss.Data.Configurations
{
    using Hss.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.OrderId)
                .IsRequired(true);

            builder.HasIndex(i => i.OrderId)
                .IsUnique(true);

            builder.Property(i => i.ClientId)
                .IsRequired(true);

            // Relations
            builder.HasOne(i => i.Client)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Address)
                .WithMany(a => a.Invoices)
                .HasForeignKey(i => i.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Order)
                .WithOne(o => o.Invoice)
                .HasForeignKey<Order>(i => i.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
