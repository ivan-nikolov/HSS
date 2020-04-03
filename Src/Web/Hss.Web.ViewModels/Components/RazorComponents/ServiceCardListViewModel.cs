namespace Hss.Web.ViewModels.Components.RazorComponents
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class ServiceCardListViewModel : IMapFrom<Service>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string ShortDescription => this.Description.Substring(0, 100) + "...";

        public string CategoryName { get; set; }

        public int? CategoryId { get; set; }
    }
}
