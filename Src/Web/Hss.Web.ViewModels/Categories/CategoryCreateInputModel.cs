namespace Hss.Web.ViewModels.Categories
{
    using Hss.Services.Mapping;
    using Hss.Services.Models.Categories;

    public class CategoryCreateInputModel : IMapTo<CategoryServiceModel>
    {
        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
