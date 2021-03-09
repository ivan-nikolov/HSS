namespace Hss.Web.ViewModels.Categories
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string ParentCategoryName { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
