namespace Hss.Web.Components.Categories
{
    using System.Linq;

    using Hss.Services.Data.Categories;
    using Hss.Services.Mapping;
    using Hss.Web.ViewModels.Categories;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesListViewComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesListViewComponent(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IViewComponentResult Invoke(int? id = null)
        {
            var categories = this.categoriesService
                .GetAllCategories();
            if (id.HasValue)
            {
                categories = categories
                    .Where(c => c.ParentCategoryId == id.Value);
            }
            else
            {
                categories = categories
                    .Where(c => c.ParentCategoryId == null);
            }

            var categoriesModel = new CategoriesListViewModel()
            {
                Categories = categories
                 .Select(c => c.To<CategoryViewModel>())
                 .ToList(),
            };

            return this.View(categoriesModel);
        }
    }
}
