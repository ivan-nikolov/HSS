namespace Hss.Data.Configurations
{
    using Hss.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);

            builder.HasIndex(s => s.Name)
                .IsUnique();

            builder.Property(s => s.Description)
                .HasMaxLength(500)
                .IsRequired(true)
                .IsUnicode(true);

            builder.Property(s => s.ImageName)
                .IsRequired(true);

            // Relations
            builder.HasOne(s => s.Category)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
