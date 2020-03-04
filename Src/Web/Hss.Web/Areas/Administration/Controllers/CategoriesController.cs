namespace Hss.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Services.Data.Categories;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Categories;
    using Hss.Web.Filters;
    using Hss.Web.ViewModels.Administration.Categories;
    using Hss.Web.ViewModels.Categories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CategoriesController : AdministrationController
    {
        private const string CategoryNotFoundErrorKey = "CategoryNotFound";
        private const string CategoryNotFoundErrorMessage = "Category Not Found";

        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult All()
        {
            var categories = new CategoriesListViewModel()
            {
                Categories = this.categoriesService
                .GetAllCategories()
                .Select(c => c.To<CategoryViewModel>()),
            };

            return this.View(categories);
        }

        public IActionResult Create()
        {
            // TODO: Refactor this
            var inputModel = new CreateCategoryInputModel()
            {
                Categories = this.categoriesService
                .GetAllRootCategories()
                .Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                }),
            };

            inputModel.Categories = inputModel.Categories.Prepend(new SelectListItem() { Text = "Select Parent Category", Value = string.Empty });

            return this.View(inputModel);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create(CreateCategoryInputModel input)
        {
            var category = input.To<CategoryServiceModel>();
            await this.categoriesService.CreateAsync(category);

            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await this.categoriesService.GetById(id);
            if (category == null)
            {
                this.ModelState.AddModelError(CategoryNotFoundErrorKey, CategoryNotFoundErrorMessage);
                return this.RedirectToAction("All");
            }

            var viewModel = category.To<CategoryViewModel>();

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Delete(DeleteCategoryInputModel input)
        {
            await this.categoriesService.DeleteAsync(input.Id);

            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await this.categoriesService.GetById(id);
            if (category == null)
            {
                this.ModelState.AddModelError(CategoryNotFoundErrorKey, CategoryNotFoundErrorMessage);
                return this.RedirectToAction("All");
            }

            var viewModel = category.To<DetailsCategoryViewModel>();
            viewModel.ChildCategories = this.categoriesService.GetAllCategories()
                .Where(c => c.ParentCategoryId == id)
                .Select(c => c.To<DetailsCategoryViewModel>())
                .ToList();

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await this.categoriesService.GetById(id);

            var viewModel = category.To<EditCategoryViewModel>();

            viewModel.Categories = this.categoriesService
                .GetAllRootCategories()
                .Where(c => c.Id != id)
                .Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                });

            viewModel.Categories = viewModel.Categories.Prepend(new SelectListItem() { Text = "Select Parent Category", Value = string.Empty });

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Edit(EditCategoryInputViewModel input)
        {
            await this.categoriesService.Update(input.To<CategoryServiceModel>());

            return this.RedirectToAction("All");
        }
    }
}
