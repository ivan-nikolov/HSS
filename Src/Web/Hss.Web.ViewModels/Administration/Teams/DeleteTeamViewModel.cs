namespace Hss.Web.ViewModels.Administration.Teams
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteTeamViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Country")]
        public string CityCountryName { get; set; }

        [Display(Name = "City")]
        public string CityName { get; set; }
    }
}
