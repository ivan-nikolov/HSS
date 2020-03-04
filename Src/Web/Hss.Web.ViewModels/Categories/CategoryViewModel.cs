namespace Hss.Web.ViewModels.Categories
{
    using Hss.Services.Mapping;
    using Hss.Services.Models.Categories;

    public class CategoryViewModel : IMapFrom<CategoryServiceModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ParentCategoryName { get; set; }
    }
}
