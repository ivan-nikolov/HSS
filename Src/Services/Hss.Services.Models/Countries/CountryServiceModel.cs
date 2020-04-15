namespace Hss.Services.Models.Countries
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class CountryServiceModel : IMapFrom<Country>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
