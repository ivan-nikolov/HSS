namespace Hss.Data.Configurations
{
    using Hss.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);

            builder.HasIndex(t => t.Name)
                .IsUnique();

            // Relations
            builder.HasOne(t => t.City)
                .WithMany(c => c.Teams)
                .HasForeignKey(t => t.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
