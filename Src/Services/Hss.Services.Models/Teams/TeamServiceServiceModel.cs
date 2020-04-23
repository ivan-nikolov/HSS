namespace Hss.Services.Models.Teams
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Services;

    public class TeamServiceServiceModel : IMapFrom<TeamService>, IMapTo<TeamService>
    {
        public int ServiceId { get; set; }

        public ServiceServiceModel Service { get; set; }

        public string TeamId { get; set; }

        public TeamServiceModel Team { get; set; }
    }
}
