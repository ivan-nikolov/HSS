namespace Hss.Web.ViewModels.Orders
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class CancelOrderAddressViewModel : IMapFrom<Address>
    {
        public string BuildingNumber { get; set; }

        public string Appartment { get; set; }

        public string StreetName { get; set; }

        public string Neighborhood { get; set; }

        public string PostCode { get; set; }

        public string CityName { get; set; }
    }
}
