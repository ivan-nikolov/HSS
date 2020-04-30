namespace Hss.Services.Notifier.EventArgs
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class AddressEventArgs : IMapFrom<Address>
    {
        public string BuildingNumber { get; set; }

        public string Appartment { get; set; }

        public string StreetName { get; set; }

        public string Neighborhood { get; set; }

        public string PostCode { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public string CityCountryName { get; set; }
    }
}
