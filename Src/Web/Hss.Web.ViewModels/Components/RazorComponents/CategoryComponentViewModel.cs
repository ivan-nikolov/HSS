namespace Hss.Web.ViewModels.Components.RazorComponents
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class CategoryComponentViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string ParentCategoryName { get; set; }

        public int? ParentCategoryId { get; set; }

        public int ChildCategoriesCount { get; set; }
    }
}
