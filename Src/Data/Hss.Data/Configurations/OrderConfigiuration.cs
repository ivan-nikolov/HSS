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
                .IsRequired(true);

            // Relations
            builder.HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Service)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Team)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Address)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AddresId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Appointment)
                .WithOne(a => a.Order)
                .HasForeignKey<Order>(o => o.AppointmetnId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
