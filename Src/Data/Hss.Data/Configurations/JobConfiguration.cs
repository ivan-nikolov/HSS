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

            // Relations
            builder.HasOne(j => j.Order)
                .WithMany(o => o.Jobs)
                .HasForeignKey(j => j.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
