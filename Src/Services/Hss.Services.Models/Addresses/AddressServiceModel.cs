namespace Hss.Services.Models.Addresses
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class AddressServiceModel : IMapFrom<Address>
    {
        public int Id { get; set; }

        public string BuildingNumber { get; set; }

        public string Appartment { get; set; }

        public string StreetName { get; set; }

        public string Neighborhood { get; set; }

        public int PostCode { get; set; }

        public int CityId { get; set; }

        public int CountryId { get; set; }
    }
}
