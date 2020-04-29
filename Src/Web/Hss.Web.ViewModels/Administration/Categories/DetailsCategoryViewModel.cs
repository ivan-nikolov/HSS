namespace Hss.Web.ViewModels.Administration.Categories
{
    using Ganss.XSS;
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class DetailsCategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string ImageUrl { get; set; }

        public int? ParentCategoryId { get; set; }

        public int ChildCategoriesCount { get; set; }
    }
}
