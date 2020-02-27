namespace Hss.Data.Configurations
{
    using Hss.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OrderConfigiuration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.ClientId)
                .IsRequired();

            // Relations
            builder.HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Contract)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ContractId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(i => i.Invoice)
            //    .WithOne(i => i.Order)
            //    .HasForeignKey<Invoice>(o => o.OrderId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
