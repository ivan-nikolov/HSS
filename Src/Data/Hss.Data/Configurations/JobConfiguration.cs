namespace Hss.Data.Configurations
{
    using Hss.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(j => j.OrderId)
                .IsRequired(true);

            builder.Property(j => j.TeamId)
                .IsRequired();

            // Relations
            builder.HasOne(j => j.Service)
                .WithMany(j => j.Jobs)
                .HasForeignKey(j => j.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(j => j.Team)
                .WithMany(j => j.Jobs)
                .HasForeignKey(j => j.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(j => j.Order)
                .WithMany(o => o.Jobs)
                .HasForeignKey(j => j.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(j => j.Address)
                .WithMany(a => a.Jobs)
                .HasForeignKey(j => j.AddresId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
