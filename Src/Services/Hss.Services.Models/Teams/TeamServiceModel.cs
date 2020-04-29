namespace Hss.Services.Models.Teams
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class TeamServiceModel : IMapFrom<Team>, IMapTo<Team>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int CityId { get; set; }

        public IEnumerable<int> Services { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Team, TeamServiceModel>()
                .ForMember(m => m.Services, e => e.MapFrom(t => t.Services.Select(s => s.ServiceId)));
        }
    }
}
