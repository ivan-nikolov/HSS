using Hss.Services.Mapping;

namespace Hss.Web.ViewModels.Administration.Users
{
    public class AssignUserToTeamTeamViewModel : IMapFrom<Hss.Data.Models.Team>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
