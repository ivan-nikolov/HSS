namespace Hss.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Hss.Services;
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
        private readonly ICloudinaryService cloudinaryService;

        public CategoriesController(ICategoriesService categoriesService, ICloudinaryService cloudinaryService)
        {
            this.categoriesService = categoriesService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult All()
        {
            return this.View();
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

            return this.View(inputModel);
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> Create(CreateCategoryInputModel input)
        {
            if (input.ParentCategoryId.HasValue)
            {
                var parantCategory = this.categoriesService.GetByIdAsync(input.ParentCategoryId.Value);
            }

            var category = input.To<CategoryServiceModel>();
            category.ImageUrl = await this.cloudinaryService.UploadAsync(input.Image);
            await this.categoriesService.CreateAsync(category);

            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await this.categoriesService.GetByIdAsync(id);
            if (category == null)
            {
                this.ModelState.AddModelError(CategoryNotFoundErrorKey, CategoryNotFoundErrorMessage);
                return this.RedirectToAction("All");
            }

            var viewModel = category.To<CategoryViewModel>();

            return this.View(viewModel);
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> Delete(DeleteCategoryInputModel input)
        {
            await this.categoriesService.DeleteAsync(input.Id);

            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await this.categoriesService.GetByIdAsync(id);

            var viewModel = category.To<DetailsCategoryViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await this.categoriesService.GetByIdAsync(id);

            var viewModel = category.To<EditCategoryViewModel>();

            viewModel.Categories = this.categoriesService
                .GetAllRootCategories()
                .Where(c => c.Id != id)
                .Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                });

            return this.View(viewModel);
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> Edit(EditCategoryInputViewModel input)
        {
            await this.categoriesService.UpdateAsync(input.To<CategoryServiceModel>());

            return this.RedirectToAction("All");
        }
    }
}
