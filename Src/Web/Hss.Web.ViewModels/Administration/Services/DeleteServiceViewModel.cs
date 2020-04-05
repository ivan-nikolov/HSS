namespace Hss.Web.ViewModels.Administration.Services
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class DeleteServiceViewModel : IMapFrom<Service>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string IamgeUrl { get; set; }
    }
}
