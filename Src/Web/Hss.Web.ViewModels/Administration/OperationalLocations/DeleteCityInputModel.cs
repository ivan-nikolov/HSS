namespace Hss.Web.ViewModels.Administration.OperationalLocations
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class DeleteCityInputModel : IMapFrom<City>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CountryName { get; set; }
    }
}
