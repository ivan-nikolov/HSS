namespace Hss.Web.ViewModels.Administration.Users
{
    using Hss.Services.Mapping;

    public class AssignUserToTeamTeamViewModel : IMapFrom<Hss.Data.Models.Team>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
