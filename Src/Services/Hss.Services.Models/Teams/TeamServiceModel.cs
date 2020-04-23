namespace Hss.Services.Models.Teams
{
    using System.Collections.Generic;

    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class TeamServiceModel : IMapFrom<Team>, IMapTo<Team>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int CityId { get; set; }

        public IEnumerable<int> Services { get; set; }
    }
}
