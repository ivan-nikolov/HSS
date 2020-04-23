namespace Hss.Web.ViewModels.Components.RazorComponents.Dashboard.Addresses
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class AddressViewModel : IMapFrom<Address>
    {
        public int Id { get; set; }

        public string BuildingNumber { get; set; }

        public string Appartment { get; set; }

        public string StreetName { get; set; }

        public string Neighborhood { get; set; }

        public int PostCode { get; set; }

        public string CityName { get; set; }

        public string CityCountryName { get; set; }
    }
}
