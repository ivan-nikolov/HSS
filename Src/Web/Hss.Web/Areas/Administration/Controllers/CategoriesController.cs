namespace Hss.Web.Areas.Administration.Controllers
{
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
            return this.View();
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> Create(CreateCategoryInputModel input)
        {
            if (input.ParentCategoryId.HasValue)
            {
                var parentCategory = await this.categoriesService.GetByIdAsync<CreateCategoryInputModel>(input.ParentCategoryId.Value);
                if (parentCategory == null)
                {
                    return this.NotFound();
                }
            }

            var category = input.To<CategoryServiceModel>();
            category.ImageUrl = await this.cloudinaryService.UploadAsync(input.Image);
            await this.categoriesService.CreateAsync(category);

            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = await this.categoriesService.GetByIdAsync<DeleteCategoryInputModel>(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> Delete(DeleteCategoryInputModel input)
        {
            var category = await this
                .categoriesService
                .GetByIdAsync<DeleteCategoryInputModel>(input.Id);

            if (category == null)
            {
                return this.NotFound();
            }

            await this.categoriesService.DeleteAsync(input.Id);

            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await this.categoriesService.GetByIdAsync<DetailsCategoryViewModel>(id);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await this.categoriesService.GetByIdAsync<CategoryServiceModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel.To<EditCategoryViewModel>());
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> Edit(EditCategoryInputModel input)
        {
            var category = await this.categoriesService.GetByIdAsync<CategoryServiceModel>(input.Id);

            if (category == null)
            {
                return this.NotFound();
            }

            await this.categoriesService.UpdateAsync(input.To<CategoryServiceModel>());

            return this.RedirectToAction("All");
        }
    }
}
