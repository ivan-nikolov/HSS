namespace Hss.Web.ViewModels.Team.Jobs
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class JobAddressViewModel : IMapFrom<Address>
    {
        public string BuildingNumber { get; set; }

        public string Appartment { get; set; }

        public string StreetName { get; set; }

        public string Neighborhood { get; set; }
    }
}
