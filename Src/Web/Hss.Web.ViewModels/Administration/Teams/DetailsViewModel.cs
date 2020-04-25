namespace Hss.Web.ViewModels.Administration.Teams
{
    using System.ComponentModel.DataAnnotations;

    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class DetailsViewModel : IMapFrom<Team>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Country")]
        public string CityCountryName { get; set; }

        [Display(Name = "City")]
        public string CityName { get; set; }

        // TODO: Add Orders History and Active Orders
    }
}
