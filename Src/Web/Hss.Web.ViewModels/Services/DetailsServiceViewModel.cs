namespace Hss.Web.ViewModels.Services
{
    using Ganss.XSS;
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class DetailsServiceViewModel : IMapFrom<Service>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public int DurationInHours { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
    }
}
