namespace Hss.Services.Models.Cities
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class CitySelectModel : IMapFrom<City>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
