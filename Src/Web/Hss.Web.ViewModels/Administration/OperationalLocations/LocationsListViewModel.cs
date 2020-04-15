namespace Hss.Web.ViewModels.Administration.OperationalLocations
{
    using System.Collections.Generic;

    public class LocationsListViewModel
    {
        public IEnumerable<LocationsIndexViewModel> Locations { get; set; }
    }
}
