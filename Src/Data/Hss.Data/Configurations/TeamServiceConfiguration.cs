namespace Hss.Data.Configurations
{
    using Hss.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TeamServiceConfiguration : IEntityTypeConfiguration<TeamService>
    {
        public void Configure(EntityTypeBuilder<TeamService> builder)
        {
            builder.HasKey(ts => new { ts.TeamId, ts.ServiceId });

            // Relations
            builder.HasOne(ts => ts.Team)
                .WithMany(t => t.Services)
                .HasForeignKey(ts => ts.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ts => ts.Service)
                .WithMany(s => s.Teams)
                .HasForeignKey(ts => ts.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
