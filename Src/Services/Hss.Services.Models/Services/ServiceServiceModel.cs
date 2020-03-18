namespace Hss.Services.Models.Services
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class ServiceServiceModel : IMapFrom<Service>, IMapTo<Service>
    {
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }
    }
}
