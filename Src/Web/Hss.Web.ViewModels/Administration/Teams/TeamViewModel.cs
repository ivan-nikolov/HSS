namespace Hss.Web.ViewModels.Administration.Teams
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class TeamViewModel : IMapFrom<Team>, IHaveCustomMappings
    {
        public TeamViewModel()
        {
            this.Services = new List<string>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Services { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Team, TeamViewModel>()
                .ForMember(m => m.Services, a => a.MapFrom(s => s.Services.Select(s => s.Service.Name)));
        }
    }
}
