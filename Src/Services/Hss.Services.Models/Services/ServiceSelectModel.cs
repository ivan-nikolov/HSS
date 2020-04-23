namespace Hss.Services.Models.Services
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class ServiceSelectModel : IMapFrom<Service>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
