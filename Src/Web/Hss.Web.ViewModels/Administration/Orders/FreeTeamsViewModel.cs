namespace Hss.Web.ViewModels.Administration.Orders
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class FreeTeamsViewModel : IMapFrom<Team>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
