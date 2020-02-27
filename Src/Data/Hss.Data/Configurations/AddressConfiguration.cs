namespace Hss.Data.Configurations
{
    using Hss.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.StreetName)
                .HasMaxLength(200)
                .IsUnicode(true);

            builder.Property(a => a.Neighborhood)
                .HasMaxLength(200)
                .IsUnicode(true);

            builder.Property(a => a.Appartment)
                .HasMaxLength(20)
                .IsUnicode(true);

            // Relations
            builder.HasOne(a => a.City)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
