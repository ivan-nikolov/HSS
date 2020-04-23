using System.ComponentModel.DataAnnotations;

namespace Hss.Web.ViewModels.Administration.Teams
{
    public class DetailsViewModel
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
