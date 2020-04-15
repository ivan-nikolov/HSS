namespace Hss.Web.ViewModels.Administration.OperationalLocations
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class DeleteCountryInputModel : IMapFrom<City>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
