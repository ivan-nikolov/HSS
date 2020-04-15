namespace Hss.Web.ViewModels.Administration.OperationalLocations
{
    using System.Collections.Generic;

    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class LocationsIndexViewModel : IMapFrom<Country>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<CityIndexViewModel> Cities { get; set; }
    }
}
