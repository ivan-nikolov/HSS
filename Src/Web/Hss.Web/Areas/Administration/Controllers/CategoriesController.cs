namespace Hss.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Services.Data.Categories;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Categories;
    using Hss.Web.Filters;
    using Hss.Web.ViewModels.Categories;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Create()
        {
            var categoriesModel = new CategoriesListViewModel()
            {
                Categories = this.categoriesService
                .GetAllRootCategories()
                .Select(c => c.To<CategoryViewModel>())
                .ToList(),
            };

            return this.View(categoriesModel);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create(CategoryCreateInputModel input)
        {
            var category = input.To<CategoryServiceModel>();
            await this.categoriesService.CreateAsync(category);

            return this.RedirectToAction("Index", "Dashboard");
        }
    }
}
